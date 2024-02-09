using Mazzatech.Domain.EntitiesModels;
using Mazzatech.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace APIMazzatech.Protocolo.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProtocoloController : ControllerBase
    {
        private readonly IProtocoloService _protocoloService;
        private readonly IConfiguration _configuration;
        public ProtocoloController(IProtocoloService protocoloService, IConfiguration configuration)
        {
            _protocoloService = protocoloService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IEnumerable<ProtocoloEntityModel>> GetProtocolo()
        {
            var proposta = await _protocoloService.GetAll();

            if (proposta == null)
            {
                return (IEnumerable<ProtocoloEntityModel>)NotFound();
            }

            return proposta;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProtocoloEntityModel>> GetProtocolo(int id)
        {
            var proposta = await _protocoloService.GetById(id);

            if (proposta == null)
            {
                return NotFound();
            }

            return proposta;
        }

        [HttpGet("Pesquisar")]
        public async Task<ActionResult<ProtocoloEntityModel>> GetProtocolo(Guid? protocolo, string? cpf, string? rg)
        {
            if (protocolo.HasValue)
            {
                var _protocolo = await _protocoloService.GetByProtocolo(protocolo);
                if (_protocolo != null)
                {
                    return _protocolo;
                }
            }

            if (!string.IsNullOrEmpty(cpf))
            {
                var _propostaCpf = await _protocoloService.GetByCPF(cpf);
                if (_propostaCpf != null)
                {
                    return _propostaCpf;
                }
            }

            if (!string.IsNullOrEmpty(rg))
            {
                var _propostaRg = await _protocoloService.GetByRG(rg);
                if (_propostaRg != null)
                {
                    return _propostaRg;
                }
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<ProtocoloEntityModel>> PostProtocolo(ProtocoloEntityModel protocoloEntityModel)
        {
            //var config = new ConfigurationBuilder()
            //.SetBasePath(Directory.GetCurrentDirectory())
            //.AddJsonFile("appSettings.json")
            //.Build();

            //string? apiUrlGet = config["ApiSettings:ApiUrlPost"];

            //if (!string.IsNullOrEmpty(apiUrlGet))
            //{
            //    using (HttpClient client = new HttpClient())
            //    {
            try
            {
                //            HttpResponseMessage getResponse = await client.GetAsync(apiUrlGet);
                //            if (getResponse.IsSuccessStatusCode)
                //            {
                //                // Converte a resposta para uma string
                //                string getResult = await getResponse.Content.ReadAsStringAsync();
                //                Console.WriteLine("GET Result: " + getResult);

                //                var dbErrorLogEntity = JsonConvert.DeserializeObject<UserCredentials>(getResult);

                await _protocoloService.Add(protocoloEntityModel);
                return CreatedAtAction("GetProtocolo", new { id = protocoloEntityModel.Id }, protocoloEntityModel);
                //            }
                //            else
                //                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            //    }
            //}
            //else
            //    return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProtocolo(int id, ProtocoloEntityModel protocoloEntityModel)
        {
            if (id != protocoloEntityModel.Id)
            {
                return BadRequest();
            }

            try
            {
                await _protocoloService.Update(protocoloEntityModel);
            }
            catch (DBConcurrencyException)
            {
                if (!(_protocoloService.GetAll().Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProtocolo(int id)
        {
            var proposta = await _protocoloService.DeleteById(id);
            if (proposta == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
