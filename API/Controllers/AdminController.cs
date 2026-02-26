using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.Usuario;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{   [Authorize(Roles = "Admin")]
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

        [HttpGet("listar-usuarios")]
        public async Task<IActionResult> ListarUsuarios()
        {
            var usuarios = await _adminService.ListarUsuarios();
            return Ok(usuarios);
        }

        [HttpPost("alterar-usuario")]
        public async Task<IActionResult> AtualizarUsuario(UsuarioAtualizarDto dto, int id)
        {
            var AdminId = User.FindFirst("id")?.Value;
            var AdminNome = User.FindFirst("Nome")?.Value;
            var result = await _adminService.AtualizarUsuario(dto, id);
            //ToDo
            //Alterar senha do usuário
            if (result)
            {
                _logger.LogWarning("Administrador ID:{AdminId}:{AdminNome} atualizou o usuário com ID:{id}", AdminId,AdminNome, id);
                return Ok("Usuário atualizado com sucesso.");
            }
            _logger.LogWarning("Administrador ID:{AdminId}:{AdminNome} tentou atualizar o usuário com ID:{id}", AdminId,AdminNome, id);
            return BadRequest("Usuario não encontrado.");
        }

        [HttpDelete("deletar-usuario/{id}")]
        public async Task<IActionResult> DeletarUsuario(int id)
        {
            var AdminId = User.FindFirst("id")?.Value;
            var AdminNome = User.FindFirst("Nome")?.Value;
            var result = await _adminService.DeletarUsuario(id);
            if (result)
            {
                _logger.LogWarning("Administrador ID:{AdminId}:{AdminNome} Deletou o usuário com ID:{id}", AdminId,AdminNome, id);
                return Ok("Usuário deletado com sucesso.");
            }
             _logger.LogWarning("Administrador ID:{AdminId}:{AdminNome} tentou deletar o usuário com ID:{id} ", AdminId,AdminNome, id);
            return BadRequest("Usuário não encontrado.");
        }
    }
}