using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API_AGENDA.Context;
using API_AGENDA.DTOs;
using API_AGENDA.Models;
using API_AGENDA.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace API_AGENDA.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        // Injeção de dependência para o contexto do banco e configuração
        private readonly AgendaContext _context;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly ItokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AgendaContext context, IPasswordHasher<Usuario> passwordHasher, ItokenService token, ILogger<AuthController> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenService = token;
            _logger = logger;
        }
        [Authorize(Roles = "Admin")]
        // Endpoint para registrar um novo usuário
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
            {

                return BadRequest("Email já cadastrado");
            }


            var usuario = new Usuario
            {
                Name = dto.Nome,
                Email = dto.Email,
                Role = dto.Role,
            };

            //cria a hash da senha
            usuario.SenhaHash = _passwordHasher.HashPassword(usuario, dto.Senha);

            _context.Usuarios.Add(usuario);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar usuário {Email}", dto.Email);
                return BadRequest("Erro ao criar usuário");
            }

            return Ok("Usuário criado com sucesso");
        }

        // Endpoint para login e geração de token JWT
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto login)
        {
            //mudar context parssar para service de autenticação
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == login.Email);

            if (usuario == null)
            {
                _logger.LogWarning("Tentativa de login com email inexistente: {Email}", login.Email);
                return Unauthorized("Email ou senha inválidos");
            }

            var resultado = _passwordHasher.VerifyHashedPassword(
                usuario,
                usuario.SenhaHash,
                login.Senha
            );

            if (resultado == PasswordVerificationResult.Failed)
            {
                _logger.LogWarning("Senha incorreta para o usuário {Email}", login.Email);
                return Unauthorized("Email ou senha inválidos");
            }


            _logger.LogWarning("Usuário {Email} fez login", login.Email);

            //token gerado acess
            var token = _tokenService.CreateToken(usuario);

            //refresh token gerado
            var refreshToken = _tokenService.refreshToken();

            var refreshTokenModel = new RefreshToken
            {
                Token = refreshToken,
                ExpiraEm = DateTime.UtcNow.AddDays(1),
                Revogado = false,
                UsuarioId = usuario.Id
            };

            _context.RefreshTokens.Add(refreshTokenModel);
            await _context.SaveChangesAsync();

            return Ok(new LoginResponseDto
            {
                Role = usuario.Role,
                Email = usuario.Email,
                Token = token,
                RefreshToken = refreshToken,

            });

        }
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult> RefreshToken(RefreshRequestDto dto)
        {
            var refresTokenDB = await _context.RefreshTokens.Include(r => r.Usuario).FirstOrDefaultAsync(r => r.Token == dto.RefreshToken);
            if(refresTokenDB == null || refresTokenDB.Revogado || refresTokenDB.ExpiraEm < DateTime.UtcNow)
            {
                return Unauthorized("Refresh token inválido");
            }

            //gera novo token acess
            var token = _tokenService.CreateToken(refresTokenDB.Usuario!);

            //revoga o refresh token antigo
            refresTokenDB.Revogado = true;

            //Gera um novo refresh token
            var novoRefreshToken = _tokenService.refreshToken();
            var refreshTokenModel = new RefreshToken
            {
                Token = novoRefreshToken,
                ExpiraEm = DateTime.UtcNow.AddHours(1),
                Revogado = false,
                UsuarioId = refresTokenDB.UsuarioId
            };

            _context.RefreshTokens.Add(refreshTokenModel);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar refresh token para usuário {Email}", refresTokenDB.Usuario.Email);
                return BadRequest("Erro ao atualizar refresh token");
            }

            return Ok(new LoginResponseDto
            {
                Email = refresTokenDB.Usuario.Email,
                Role = refresTokenDB.Usuario.Role,
                Token = token,
                RefreshToken = novoRefreshToken
            });





        }



    }
}