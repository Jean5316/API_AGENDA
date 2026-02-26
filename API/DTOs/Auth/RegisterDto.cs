using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace API_AGENDA.DTOs
{
    public class RegisterDto
    {
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "O campo Nome não pode conter e-mails ou caracteres especiais.")]
        [DefaultValue("string")]
        [MaxLength(100)]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email em formato inválido")]
        [MaxLength(150)]
        [DefaultValue("string")]
        public string? Email { get; set; }

        [JsonPropertyName("Senha")]
        [Required(ErrorMessage = "A senha é obrigatória")]
        // Validação de Tamanho
        [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres")]
        [MaxLength(20, ErrorMessage = "A senha deve ter no máximo 20 caracteres")]
         // Validação de Complexidade (Regex)
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "A senha deve conter: 1 maiúscula, 1 minúscula, 1 número e 1 caractere especial.")]
        [DefaultValue("string")]
        public string? Senha { get; set; }


        [JsonPropertyName("tipo de usuario")]
        [Required(ErrorMessage = "O tipo de usuario é obrigatório.")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "O campo tipo de usuario não pode conter e-mails ou caracteres especiais.")]
        [DefaultValue("string")]
        public string? Role { get; set; }
    }
}