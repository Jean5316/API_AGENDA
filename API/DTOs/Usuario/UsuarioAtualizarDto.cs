using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Usuario
{
    public class UsuarioAtualizarDto
    {


        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        //Implementar alteração de senha
        // [Required]
        // public string SenhaHash { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
        public bool Ativo { get; set; }



    }
}