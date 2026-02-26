using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs.Usuario;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;
        public AdminController(IAdminService adminService, ILogger<AdminController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        //função para pegar o id do administrador logado a partir dos claims do token JWT
        private string getAdminId()
        {
            return User.FindFirst("id")?.Value ?? "0";
        }

        //função para pegar o nome do administrador logado a partir dos claims do token JWT
        private string getAdminNome()
        {
            return User.FindFirst("Nome")?.Value ?? "Desconhecido";
        }

        //função para pegar o email do administrador logado a partir dos claims do token JWT
        private string getAdminEmail()
        {
            return User.FindFirst("Email")?.Value ?? "Desconhecido";
        }

        [HttpGet("listar-usuarios")]
        public async Task<IActionResult> ListarUsuarios()
        {
            var adminId = getAdminId();
            var adminNome = getAdminNome();
            var adminEmail = getAdminEmail();
            _logger.LogInformation("Administrador ID:{AdminId} Nome:{AdminNome} Email:{AdminEmail} solicitou listagem de todos os usuários", adminId, adminNome, adminEmail);
            var usuarios = await _adminService.ListarUsuarios();
            return Ok(usuarios);
        }

        [HttpPost("alterar-usuario")]
        public async Task<IActionResult> AtualizarUsuario(UsuarioAtualizarDto dto, int id)
        {
            var adminId = getAdminId();
            var adminNome = getAdminNome();
            var adminEmail = getAdminEmail();
            var result = await _adminService.AtualizarUsuario(dto, id);
            //ToDo
            //Alterar senha do usuário
            if (result)
            {
                _logger.LogInformation("Administrador ID:{AdminId} Nome:{AdminNome} Email:{AdminEmail} atualizou o usuário com ID:{id}", adminId, adminNome, adminEmail, id);
                return Ok("Usuário atualizado com sucesso.");
            }
            _logger.LogWarning("Administrador ID:{AdminId} Nome:{AdminNome} Email:{AdminEmail} tentou atualizar o usuário com ID:{id}", adminId, adminNome, adminEmail, id);
            return BadRequest("Usuario não encontrado.");
        }

        [HttpDelete("deletar-usuario/{id}")]
        public async Task<IActionResult> DeletarUsuario(int id)
        {
            var adminId = getAdminId();
            var adminNome = getAdminNome();
            var adminEmail = getAdminEmail();
            var result = await _adminService.DeletarUsuario(id);
            if (result)
            {
                _logger.LogInformation("Administrador ID:{AdminId} Nome:{AdminNome} Email:{AdminEmail} deletou o usuário com ID:{id}", adminId, adminNome, adminEmail, id);
                return Ok("Usuário deletado com sucesso.");
            }
            _logger.LogWarning("Administrador ID:{AdminId} Nome:{AdminNome} Email:{AdminEmail} tentou deletar o usuário com ID:{id}", adminId, adminNome, adminEmail, id);
            return BadRequest("Usuário não encontrado.");
        }
    }
}
