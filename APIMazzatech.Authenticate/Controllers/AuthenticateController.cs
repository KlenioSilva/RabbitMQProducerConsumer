using Mazzatech.CrossCutting;
using Mazzatech.Domain.EntitiesModels;
using Mazzatech.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIMazzatech.Authenticate.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IIssuerSecretKeyService _issuerSecretKeyService;
        private readonly IIssuerUserSecretKeyTokenService _issuerSecretKeyTokenService;
        private readonly IConfiguration _configuration;
        public AuthenticateController(IUserService userService, IIssuerSecretKeyService issuerSecretKeyService, IIssuerUserSecretKeyTokenService issuerSecretKeyTokenService, IConfiguration configuration)
        {
            _userService = userService;
            _issuerSecretKeyService = issuerSecretKeyService;
            _issuerSecretKeyTokenService = issuerSecretKeyTokenService;
            _configuration = configuration;
        }

        [HttpGet("GetAuthenticate")]
        public async Task<IActionResult> GetAuthenticate(string UserName, string Password)
        {
            UserEntityModel userEntityModel = new UserEntityModel();
            userEntityModel.UserName = UserName;
            userEntityModel.Password = Password;

            // Verificar credenciais e autentica o usuário
            if (_userService.IsValidUser(userEntityModel))
            {
                return Ok(new { Token = _issuerSecretKeyTokenService.GetTokenById(userEntityModel.Id) });
            }
            else
                return Problem("Favor informar UserName e Password corretos; caso seja seu primeiro acesso: use o endpoint PostUser para se cadastrar.");
        }

        [HttpPost("PostUser")]
        public async Task<string> PostUser(UserEntityModel userEntityModel)
        {
            userEntityModel.FlagActive = 1;

            await _userService.Add(userEntityModel);

            // Configuração do JWT
            // Buscar chaveSecreta do Emissor
            var _secret = await _issuerSecretKeyService.GetSecretKeyByIssuer(_configuration["Jwt:IssuerPostUser"]);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userEntityModel.UserName),
                // Adicione quaisquer outras claims necessárias
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:IssuerPostUser"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationMinutes"])),
                signingCredentials: creds
            );

            IssuerUserSecretKeyTokenEntityModel issuerSecretKeyToken = new IssuerUserSecretKeyTokenEntityModel();
            issuerSecretKeyToken.IssuerSecretKeyId = _secret.Id;
            issuerSecretKeyToken.UserId = userEntityModel.Id;
            issuerSecretKeyToken.Token = token.ToString();
            issuerSecretKeyToken.FlagActive = 1;

            await _issuerSecretKeyTokenService.Add(issuerSecretKeyToken);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("PostIssuer")]
        public async Task<IActionResult> PostIssuer(IssuerSecretKeyEntityModel issuerSecretKeyEntityModel, string UserName, string Password)
        {
            issuerSecretKeyEntityModel.FlagActive = 1;

            UserEntityModel userEntityModel = new UserEntityModel();
            userEntityModel.UserName = UserName;
            userEntityModel.Password = Password;

            if (_userService.IsValidUser(userEntityModel))
            {
                await _issuerSecretKeyService.Add(issuerSecretKeyEntityModel);

                return Ok(new { Issuer = _issuerSecretKeyService.GetById(issuerSecretKeyEntityModel.Id) });
            }
            else
                return Problem("Favor informar UserName e Password corretos; caso seja seu primeiro acesso: use o endpoint PostUser para se cadastrar.");
        }

        [HttpGet("GetUser")]
        public async Task<IEnumerable<UserEntityModel>> GetUser()
        {
            var user = await _userService.GetAll();

            if (user == null)
            {
                return (IEnumerable<UserEntityModel>)NotFound();
            }

            return user;
        }
    }
}
