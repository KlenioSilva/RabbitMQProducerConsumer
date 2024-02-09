using Mazzatech.Data.Repositories;
using Mazzatech.Domain.EntitiesModels;
using Mazzatech.Domain.Interfaces.Repositories;
using Mazzatech.Domain.Interfaces.Services;
using Mazzatech.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Reflection;
using System.Text;

namespace RMQProtocoloMazzatech
{
    public class Program
    {
        static string propertyNameDbLogger = "";
        static string message = string.Empty;
        static int qtdeMsgRecebidas = 0;

        static void Main(string[] args)
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File($"C:\\Projetos\\Mazzatech\\Sln.Mazzatech.ProtocolRobot\\LogsErrors\\LogErrorConsumer.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

                var serviceProvider = new ServiceCollection()
                    .AddLogging(loggingBuilder =>
                    {
                        loggingBuilder.AddSerilog();
                    })
                    .AddScoped<IProtocoloRepository, ProtocoloRepository>()
                    .AddScoped<IProtocoloService, ProtocoloService>()
                    .BuildServiceProvider();

                // Este método será chamado a cada intervalo do Timer
                Console.WriteLine($"Hora atual: {DateTime.Now.ToString("HH:mm:ss")}");

                // Configurações de conexão
                var factory = new ConnectionFactory()
                {
                    HostName = "localhost", // ou o IP do servidor RabbitMQ
                    Port = 5672,             // a porta padrão do RabbitMQ
                    UserName = "klenioleite",
                    Password = "151213@2024Gmc"
                };

                // Cria uma conexão
                using (var connection = factory.CreateConnection())
                // Cria um canal
                using (var channel = connection.CreateModel())
                {
                    string exchangeName = "exProtocolo";
                    string queueName = "queueProtocolo";

                    // Verificar se a fila existe
                    if (QueueExists(channel, queueName))
                    {
                        Console.WriteLine($"A fila '{queueName}' já existe.");
                    }
                    else
                    {
                        Console.WriteLine($"A fila '{queueName}' não existe. Criando...");

                        using (var connectionAux = factory.CreateConnection())
                        using (var channelAux = connection.CreateModel())
                        {
                            var maxLengthBytes = 1024; // Defina o tamanho máximo desejado em bytes

                            // Argumentos para a configuração da fila
                            var arguments = new Dictionary<string, object>
                            {
                                //{ "max-length", 0 }, // 0 significa que a política não deve ser baseada no número de mensagens
                                { "max-length-bytes", maxLengthBytes }
                            };

                            // Cria a fila se não existir
                            channelAux.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: arguments);
                        }

                        Console.WriteLine($"A fila '{queueName}' foi criada com sucesso.");
                    }

                    // Verificar se a exchange existe
                    if (ExchangeExists(channel, exchangeName))
                    {
                        Console.WriteLine($"A exchange '{exchangeName}' já existe.");
                    }
                    else
                    {
                        Console.WriteLine($"A exchange '{exchangeName}' não existe. Criando...");

                        using (var connectionAux = factory.CreateConnection())
                        using (var channelAux = connection.CreateModel())
                        {
                            // Cria a exchange se não existir
                            channelAux.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

                            // Cria o Bind, ligando a fila a exchange
                            channelAux.QueueBind(queue: queueName, exchange: exchangeName, routingKey: "protocolo");
                        }

                        Console.WriteLine($"A exchange '{exchangeName}' foi criada com sucesso.");
                    }

                    // Configura o consumidor
                    var consumer = new EventingBasicConsumer(channel);

                    // Consumidor 
                    consumer.Received += (model, ea) =>
                    {
                        try
                        {
                            var retries = GetRetryCount(ea.BasicProperties.Headers);

                            // Lógica de processamento da mensagem
                            bool success = ProcessMessage(ea, channel);

                            if (success)
                            {
                                // Aqui você pode usar o message, gerado a partir do ProcessMessage para enviar e-mails, com o conteúdo da mensagem, etc.
                                Console.WriteLine($"Mensagem recebida com sucesso. Nº de Mensagens: {qtdeMsgRecebidas}.");
                            }
                            else if (retries <= 3)
                            {
                                // Rejeitar a mensagem e incrementar o contador de tentativas.
                                RejectWithRetry(ea, retries + 1, channel);
                            }
                            else
                            {
                                // Atingiu o limite de tentativas, descartar ou lidar de acordo.
                                Log.Logger.Error($"Limite de 3 tentativas atingido. Protocolo: {propertyNameDbLogger} Fila: {queueName}, Exchange: {exchangeName};");
                            }
                        }
                        catch (Exception ex)
                        {
                            // Adicione logs para exceções não tratadas
                            Log.Logger.Error(ex, "Erro ao processar a mensagem.");
                        }
                    };

                    channel.BasicConsume(queue: queueName,
                    autoAck: false,
                    consumer: consumer);

                    Console.WriteLine("Aguardando mensagens para a routing key protocolo. Pressione [Enter] para sair.");
                    Console.ReadLine();  // Aguarda até que o usuário pressione Enter
                }
                Console.WriteLine($"Hora atual: {DateTime.Now.ToString("HH:mm:ss")}");
            }
            catch (Exception ex)
            {
                // Adicione logs para exceções não tratadas
                Log.Logger.Error($"Process Message Error: {ex}");
            }
        }

        // Método para receber a mensagem
        static bool ProcessMessage(BasicDeliverEventArgs ea, IModel channel)
        {
            try
            {
                var body = ea.Body.ToArray();
                message = Encoding.UTF8.GetString(body);

                // Após processar a mensagem com sucesso, envie a confirmação manualmente
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

                qtdeMsgRecebidas += 1;

                return true;
            }
            catch (Exception ex)
            {
                // Adicione logs para exceções não tratadas
                Log.Logger.Error($"Process Message Error: {ex}");
                return false;
            }
        }

        // Método para obter o número de tentativas de processamento da mensagem
        static int GetRetryCount(IDictionary<string, object> headers)
        {
            if (headers != null && headers.ContainsKey("retries") && headers["retries"] is int retries)
            {
                return retries;
            }
            return 0;
        }

        // Método para rejeitar a mensagem com um contador de tentativas atualizado
        static void RejectWithRetry(BasicDeliverEventArgs ea, int retries, IModel channel)
        {
            try
            {
                var body = ea.Body.ToArray();
                message = Encoding.UTF8.GetString(body);

                var properties = ea.BasicProperties;
                properties.Headers ??= new Dictionary<string, object>();
                properties.Headers["retries"] = retries;

                channel.BasicReject(ea.DeliveryTag, requeue: true);

                DbLoggerError<DbErrorLoggerEntityModel>(message, "Protocolo");
            }
            catch (Exception ex)
            {
                // Adicione logs para exceções não tratadas [arquivo de log]
                Log.Logger.Error(ex, "Erro ao processar a mensagem.");
            }
        }

        static async void DbLoggerError<T>(string? errorMessage, string? documentKey)
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json")
            .Build();

            //string? apiUrlGet = config["ApiSettings:ApiUrlGet"];
            string? apiUrlPost = config["ApiSettings:ApiUrlPost"];

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    //HttpResponseMessage getResponse = await client.GetAsync(apiUrlGet);

                    //if (getResponse.IsSuccessStatusCode)
                    //{
                    //    // Converte a resposta para uma string
                    //    string getResult = await getResponse.Content.ReadAsStringAsync();
                    //    Console.WriteLine("GET Result: " + getResult);

                    // var dbErrorLogEntity = JsonConvert.DeserializeObject<DbErrorLoggerEntityModel>(getResult);

                    // Criei esse trecho de código usando o Reflection, a fim de buscar valor de uma coleção que já contenha dados e que seja passada
                    // pelo método genérico, por meio do parâmetro nullable T
                    string? propertyValue = null;
                    string? json = $"[{message}]";
                    object? obj = (typeof(T).GetType());

                    obj = JsonConvert.DeserializeObject<T>(json);
                    Type type = obj.GetType();

                    PropertyInfo[] propriedades = type.GetProperties();
                    foreach (PropertyInfo propriedade in propriedades)
                    {
                        // Aqui podemos obter a entidade com os dados para que se procure pela documentKey e se obtenha o documentContent de cada linha
                        if (propriedade.Name == documentKey)
                            propertyValue = propriedade.GetValue(obj) as string;
                    }

                    DbErrorLoggerEntityModel dbErrorLogEntity = new DbErrorLoggerEntityModel();
                    dbErrorLogEntity.DocumentKey = documentKey;
                    dbErrorLogEntity.DocumentContent = propertyValue;
                    dbErrorLogEntity.Queue = "queueProtocolo";
                    dbErrorLogEntity.Exchange = "exProtocolo";
                    dbErrorLogEntity.ErrorMessage = errorMessage;

                    // Converte a instância do objeto (DbErrorLoggerEntityModel) em JSON
                    string jsonBody = JsonConvert.SerializeObject(dbErrorLogEntity);

                    // Realiza uma solicitação POST com os dados obtidos da mensagem advinda do RabbitMQ
                    HttpResponseMessage putResponse = await client.PostAsync(apiUrlPost, new StringContent(jsonBody, Encoding.UTF8, "application/json"));

                    if (putResponse.IsSuccessStatusCode)
                    {
                        string putResult = await putResponse.Content.ReadAsStringAsync();
                        Console.WriteLine("POST Result: " + putResult);
                        Log.Logger.Error("POST Result: " + putResult);
                    }
                    else
                    {
                        Console.WriteLine($"Erro na solicitação POST: {putResponse.StatusCode}");
                        Log.Logger.Error($"Erro na solicitação POST: {putResponse.StatusCode}");
                    }
                    //}
                    //else
                    //{
                    //    Console.WriteLine($"Erro na solicitação GET: {getResponse.StatusCode}");
                    //}
                }
                catch (Exception ex)
                {
                    Log.Logger.Error($"Erro: {ex.Message}");
                    Console.WriteLine($"Erro: {ex.Message}");
                }
            }
        }
        
        static bool QueueExists(IModel channel, string queueName)
        {
            try
            {
                var declareOk = channel.QueueDeclarePassive(queueName);
                return true; // A fila existe
            }
            catch (Exception)
            {
                return false; // A fila não existe
            }
        }

        static bool ExchangeExists(IModel channel, string exchangeName)
        {
            try
            {
                channel.ExchangeDeclarePassive(exchangeName);
                return true; // A exchange existe
            }
            catch (Exception)
            {
                return false; // A exchange não existe
            }
        }
    }
}

