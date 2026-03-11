using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using API_AGENDA.Models;

namespace API_AGENDA.DTOs
{
    public class ContatoAtualizarDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "O campo Nome não pode conter e-mails ou caracteres especiais.")]
        [DefaultValue("string")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Numero de telefone obrigatório")]
        [Phone]
        [MaxLength(20)]
        [RegularExpression(@"^\(\d{2}\)\d{4,5}-\d{4}$",
        ErrorMessage = "Telefone inválido.")]
        [DefaultValue("string")]
        public string Telefone { get; set; } = string.Empty;
        
        [MaxLength(150)]
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email em formato inválido")]
        [DefaultValue("string")]

        public string Email { get; set; } = string.Empty;
        public EnumCategorias Categoria { get; set; } = EnumCategorias.Padrao;
        
        [DefaultValue(false)]
        public bool Favorito { get; set; } = false;
    }
}
