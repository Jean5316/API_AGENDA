using API_AGENDA.Models;
using API_AGENDA.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API_AGENDA.Services
{
    public class TokenService : ItokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(Usuario usuario)
        {
            var jwtSettings = _config.GetSection("Jwt");//informando Jwt do Appsettings
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);//chave de segurança para assinar o token

            //claims informações que nao no token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Role),
                new Claim("id", usuario.Id.ToString()),
            };

            //gerando token
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)//credencias de assinatura do token
            );
            return new JwtSecurityTokenHandler().WriteToken(token);//retornando token gerado
        }

        public string refreshToken()
        {
            var randomNumber = new byte[32];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
