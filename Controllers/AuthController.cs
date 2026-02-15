using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API_AGENDA.Context;
using API_AGENDA.DTOs;
using API_AGENDA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API_AGENDA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // Injeção de dependência para o contexto do banco e configuração
        private readonly AgendaContext _context;
        private readonly IConfiguration _config;

        public AuthController(AgendaContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // Endpoint para registrar um novo usuário
        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            if (_context.Usuarios.Any(u => u.Email == dto.Email))
            return BadRequest("Email já cadastrado");

            var usuario = new Usuario
            {
            Name = dto.Nome,
            Email = dto.Email,
            Senha = dto.Senha, // depois a gente faz hash
            Role = "User"
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return Ok("Usuário criado com sucesso");
        }

        // Endpoint para login e geração de token JWT
        [HttpPost("login")]
        public IActionResult Login(LoginDto login)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == login.Email && u.Senha == login.Senha);
            if (usuario == null)
            {
                return Unauthorized("Usuario ou senha inválidos.");
            }

            var token = GenerateJwtToken(usuario);
            return Ok(new { Token = token });

        }

        // Método para gerar o token JWT
        private string GenerateJwtToken(Usuario usuario)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim(ClaimTypes.Email, usuario.Role),
                new Claim("id", usuario.Id.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
            
        
    }
}