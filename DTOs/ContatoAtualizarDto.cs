using System.ComponentModel.DataAnnotations;

namespace API_AGENDA.DTOs
{
    public class ContatoAtualizarDto
    {
        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [Phone]
        [MaxLength(20)]
        public string Telefone { get; set; } = string.Empty;
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Categoria { get; set; } = string.Empty;
        public bool Favorito { get; set; }
    }
}
