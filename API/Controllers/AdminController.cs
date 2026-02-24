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
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
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
            var result = await _adminService.AtualizarUsuario(dto, id);
            //ToDo
            //Alterar senha do usuário
            if (result)
            {
                return Ok("Usuário atualizado com sucesso.");
            }
            return BadRequest("Usuario não encontrado.");
        }

        [HttpDelete("deletar-usuario/{id}")]
        public async Task<IActionResult> DeletarUsuario(int id)
        {
            var result = await _adminService.DeletarUsuario(id);
            if (result)
            {
                return Ok("Usuário deletado com sucesso.");
            }
            return BadRequest("Usuário não encontrado.");
        }
    }
}