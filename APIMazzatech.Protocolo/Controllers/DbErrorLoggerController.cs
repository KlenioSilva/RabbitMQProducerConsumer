using Mazzatech.Domain.EntitiesModels;
using Mazzatech.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Mazzatech.Domain.Services;


namespace APIMazzatech.Protocolo.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DbErrorLoggerController : Controller
    {
        private readonly IDbErrorLoggerService _dbErrorLoggerService;
        private readonly IConfiguration _configuration;
        public DbErrorLoggerController(IDbErrorLoggerService dbErrorLoggerService, IConfiguration configuration)
        {
            _dbErrorLoggerService = dbErrorLoggerService;
            _configuration = configuration;
        }

        [HttpGet("GetDbErrorLogger")]
        public async Task<IEnumerable<DbErrorLoggerEntityModel>> GetDbErrorLogger()
        {
            var proposta = await _dbErrorLoggerService.GetAll();

            if (proposta == null)
            {
                return (IEnumerable<DbErrorLoggerEntityModel>)NotFound();
            }

            return proposta;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DbErrorLoggerEntityModel>> GetDbErrorLogger(int id)
        {
            var dbErrorLoggerEntity = await _dbErrorLoggerService.GetById(id);

            if (dbErrorLoggerEntity == null)
            {
                return NotFound();
            }

            return dbErrorLoggerEntity;
        }

        /*
            documentContent é o valor referente a chave (documentKey), 
            ex.: documentKey = Protocolo, documentContent = 8008BFE2-BC67-4327-ACBD-E08904519A5D 
        */
        [HttpGet("Pesquisar")]
        public async Task<IActionResult> GetDbErrorLogger(string? documentContent, string? queue, string? exchange, string? routingKey)
        {
            IActionResult actionResult = NoContent();

            if (!documentContent.IsNullOrEmpty())
            {
                // Use await para evitar bloqueios desnecessários
                var docContDbErrorLoggers = await _dbErrorLoggerService.GetAll();

                if (docContDbErrorLoggers == null)
                    actionResult = NotFound("Nenhum registro encontrado.");
                else
                {
                    var docContfilteredDbErrorLoggers = docContDbErrorLoggers
                            .Where(x => x.DocumentContent == documentContent)
                            .ToList();

                    if (docContfilteredDbErrorLoggers.Any())
                        actionResult = Ok(docContfilteredDbErrorLoggers);
                    else
                        actionResult = NotFound("Nenhum registro correspondente encontrado.");
                }
            }
            
            if (!queue.IsNullOrEmpty())
            {
                actionResult = BadRequest("O parâmetro 'queue' não pode ser nulo ou vazio.");

                // Use await para evitar bloqueios desnecessários
                var queueDbErrorLoggers = await _dbErrorLoggerService.GetAll();

                if (queueDbErrorLoggers == null)
                    return NotFound("Nenhum registro encontrado.");
                else
                {
                    var queueFilteredDbErrorLoggers = queueDbErrorLoggers
                        .Where(x => x.Queue == queue)
                        .ToList();

                    if (queueFilteredDbErrorLoggers.Any())
                        return Ok(queueFilteredDbErrorLoggers);
                    else
                        return NotFound("Nenhum registro correspondente encontrado.");
                }
            }

            if (!exchange.IsNullOrEmpty())
            {
                actionResult = BadRequest("O parâmetro 'queue' não pode ser nulo ou vazio.");

                // Use await para evitar bloqueios desnecessários
                var exchangeDbErrorLoggers = await _dbErrorLoggerService.GetAll();

                if (exchangeDbErrorLoggers == null)
                    return NotFound("Nenhum registro encontrado.");
                else
                {
                    var exchangeFilteredDbErrorLoggers = exchangeDbErrorLoggers
                        .Where(x => x.Exchange == exchange)
                        .ToList();

                    if (exchangeFilteredDbErrorLoggers.Any())
                        return Ok(exchangeFilteredDbErrorLoggers);
                    else
                        return NotFound("Nenhum registro correspondente encontrado.");
                }
            }

            if (!routingKey.IsNullOrEmpty())
            {
                actionResult = BadRequest("O parâmetro 'queue' não pode ser nulo ou vazio.");

                // Use await para evitar bloqueios desnecessários
                var routingKeyDbErrorLoggers = await _dbErrorLoggerService.GetAll();

                if (routingKeyDbErrorLoggers == null)
                    return NotFound("Nenhum registro encontrado.");
                else
                {
                    var routingKeyFilteredDbErrorLoggers = routingKeyDbErrorLoggers
                        .Where(x => x.RoutingKey == routingKey)
                        .ToList();

                    if (routingKeyFilteredDbErrorLoggers.Any())
                        return Ok(routingKeyFilteredDbErrorLoggers);
                    else
                        return NotFound("Nenhum registro correspondente encontrado.");
                }
            }

            return actionResult;
        }

        [HttpPost("PostDbErrorLogger")]
        [Authorize]  // Adicionando o atributo de autorização
        public async Task<ActionResult<DbErrorLoggerEntityModel>> PostDbErrorLogger(DbErrorLoggerEntityModel dbErrorLoggerController)
        {
            try
            {
                // Certifique-se de que o usuário está autenticado antes de prosseguir
                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized();  // Retorna 401 Unauthorized se o usuário não estiver autenticado
                }

                await _dbErrorLoggerService.Add(dbErrorLoggerController);

                return CreatedAtAction("GetDbErrorLogger", new { id = dbErrorLoggerController.Id }, dbErrorLoggerController);
            }
            catch (Exception ex)
            {
                // Lida com exceções, se necessário
                return BadRequest(new { error = ex.Message });
            }
        }

        
    }
}
