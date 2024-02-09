using Mazzatech.CrossCutting;
using Mazzatech.Data.EF;
using Mazzatech.Data.Repositories;
using Mazzatech.Domain.EntitiesModels;
using Mazzatech.Domain.Interfaces.Repositories;
using Mazzatech.Domain.Interfaces.Services;
using Mazzatech.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using Timer = System.Threading.Timer;

namespace RMQProtocoloMazzatech
{
    public class Program
    {
        private readonly IProtocoloService _protocoloService;
        public Program(IProtocoloService protocoloService)
        {
            _protocoloService = protocoloService;
        }

        static void Main(string[] args)
        {
            TimerCallback callback = new TimerCallback(Tick);
            Timer stateTimer = new Timer(callback, null, 0, 6000000); // A cada 10 minutos "gera" (moc, temporário) e verifica

            Console.ReadLine();
        }

        static void Tick(object? state)
        {
            var serviceProvider = new ServiceCollection()
                .AddScoped<Context>()
                .AddScoped<Program>()
                .AddScoped<IProtocoloRepository, ProtocoloRepository>()
                .AddScoped<IProtocoloService, ProtocoloService>()
                .BuildServiceProvider();

            var program = serviceProvider.GetService<Program>();

            // Este método será chamado a cada intervalo do Timer
            Console.WriteLine($"Hora atual: {DateTime.Now.ToString("HH:mm:ss")}");

            //Esse trecho moc foi criado apenas para realizar os inserts simulando as entradas de dados para utilização do cenário do robô
            string rg;
            ProtocoloEntityModel protocoloEntityModel;
            for (int i = 1; i <= 500; i++)
            {
                rg = Util.GerarRGMoque();
                protocoloEntityModel =
                new ProtocoloEntityModel()
                {
                    Protocolo = Guid.NewGuid(),
                    Nome = $"Dono(a) do RG {rg}",
                    CPF = Util.GerarCPFMoque(),
                    RG = rg,
                    Via = Util.ObterRegistro(i)._Via,
                    Motivo = Util.ObterRegistro(i)._Motivo,
                    NomeMae = $"Mãe do Dono do RG {rg}",
                    NomePai = $"Pai do Dono do RG {rg}",
                    Foto = Util.LerImagemComoBytes("C:\\Projetos\\Mazzatech\\Sln.Mazzatech.ProtocolRobot\\images\\LindoIpê.jpg")
                };

                program._protocoloService.AddWithoutReturn(protocoloEntityModel);

                if (i == 1) 
                    Console.WriteLine($"{i} Registro incluído no banco de dados.");
                else
                    Console.WriteLine($"{i} Registros incluídos no banco de dados.");
            }

            var factory = new ConnectionFactory()
            {
                HostName = "localhost", // ou o IP do servidor RabbitMQ
                Port = 5672,             // a porta padrão do RabbitMQ
                UserName = "klenioleite",
                Password = "151213@2024Gmc"
            };

            var lstProtocolo = program._protocoloService.GetAll().Result.Where(x => x.flagEnviado == 0).ToList();

            // Cria uma conexão
            using (var connection = factory.CreateConnection())
            // Cria um canal
            using (var channel = connection.CreateModel())
            {
                string exchangeName = "exProtocolo";
                string queueName = "queueProtocolo";
                IBasicProperties bProperties = null;
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
                        // Cria a fila se não existir
                        channelAux.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
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
                        bProperties = channelAux.CreateBasicProperties();
                        bProperties.Persistent = true;

                        // Cria a exchange se não existir
                        channelAux.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

                        // Cria o Bind, ligando a fila a exchange
                        channelAux.QueueBind(queueName, exchangeName, routingKey: "protocolo");
                    }

                    Console.WriteLine($"A exchange '{exchangeName}' foi criada com sucesso.");
                }

                try
                {
                    foreach (var protocoloEntityModelAux in lstProtocolo)
                    {
                        var jsonString = JsonConvert.SerializeObject(protocoloEntityModelAux);

                        var body = Encoding.UTF8.GetBytes(jsonString);

                        channel.BasicPublish(exchange: "exProtocolo",
                                             routingKey: "protocolo",
                                             basicProperties: bProperties,
                                             body: body);

                        Console.WriteLine("Mensagem enviada: {0}", jsonString);

                        protocoloEntityModelAux.flagEnviado = 1; //true
                        program._protocoloService.Update(protocoloEntityModelAux);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.ReadLine();
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
