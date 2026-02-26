using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_AGENDA.DTOs
{
    public class ContatoCriarDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "O campo Nome não pode conter e-mails ou caracteres especiais.")]
        [DefaultValue("string")]
        public string? Nome { get; set;}

        [Required(ErrorMessage = "Telefone é obrigatório.")]
        [Phone(ErrorMessage = "Numero de Telefone inválido.")]
        [MaxLength(20)]
        [RegularExpression(@"^\(\d{2}\)\d{4,5}-\d{4}$",
        ErrorMessage = "Telefone inválido.")]
        [DefaultValue("string")]
        public string? Telefone { get; set;}

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email em formato inválido")]
         [MaxLength(150)]
         [DefaultValue("string")]
        public string? Email { get; set;}

        [MaxLength(50)]
        [DefaultValue("string")]
        public string? Categoria { get; set;}

        [DefaultValue(false)]
        public bool Favorito { get; set;}

    }
}