using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Usuario
{
    public class UsuarioAtualizarDto
    {


        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "O campo Nome não pode conter e-mails ou caracteres especiais.")]
        [DefaultValue("string")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email em formato inválido")]
        [MaxLength(150)]
        [DefaultValue("string")]
        public string Email { get; set; } = string.Empty;

        //Implementar alteração de senha
        // [Required]
        // public string SenhaHash { get; set; } = string.Empty;
        [Required(ErrorMessage = "O Role é obrigatório.")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "O campo Role não pode conter e-mails ou caracteres especiais.")]
        [DefaultValue("string")]
        public string Role { get; set; } = string.Empty;

        [DefaultValue(true)]
        public bool Ativo { get; set; } = true;



    }
}