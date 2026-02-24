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
        public string Nome { get; set;}

        [Required(ErrorMessage = "Telefone é obrigatório.")]
        [Phone(ErrorMessage = "Numero de Telefone inválido.")]
        [MaxLength(20)]
        [RegularExpression(@"^\(\d{2}\)\d{4,5}-\d{4}$",
        ErrorMessage = "Telefone inválido.")]
        public string Telefone { get; set;}
        [EmailAddress]
        [MaxLength(150)]
        public string? Email { get; set;}
        [MaxLength(50)]
        public string? Categoria { get; set;}

        [DefaultValue(false)]
        public bool Favorito { get; set;}

    }
}