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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public AuthController(AgendaContext context, IConfiguration config, IPasswordHasher<Usuario> passwordHasher)
        {
            _context = context;
            _config = config;
            _passwordHasher = passwordHasher;
        }

        // Endpoint para registrar um novo usuário
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
            return BadRequest("Email já cadastrado");

            var usuario = new Usuario
            {
            Name = dto.Nome,
            Email = dto.Email,
            Role = "User"
            };

            //cria a hash da senha
            usuario.SenhaHash = _passwordHasher.HashPassword(usuario, dto.Senha);

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return Ok("Usuário criado com sucesso");
        }

        // Endpoint para login e geração de token JWT
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto login)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == login.Email);//verifica email
            if (usuario == null) return Unauthorized("Email ou senha Invalidos");
            
            var res = _passwordHasher.VerifyHashedPassword(
                usuario,
                usuario.SenhaHash,
                login.Senha
                );//verifica senha

            if(res == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Credenciais Invalidas");
            }//verifica se email e senha estao corretos

            var token = GenerateJwtToken(usuario);

            return Ok(new { Token = token });

        }

        // Método para gerar o token JWT
        private string GenerateJwtToken(Usuario usuario)
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
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)//credencias de assinatura do token
            );
            return new JwtSecurityTokenHandler().WriteToken(token);//retornando token gerado
        }
            
        
    }
}