using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_AGENDA.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email é necessario!")]
        [EmailAddress(ErrorMessage = "Email em formato inválido")]
        [MaxLength(100)]
        [DefaultValue("string")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Senha é necessaria!")]
        [DefaultValue("string")]
        [MaxLength(20)]
        public string Senha { get; set; } = string.Empty;
    }
}