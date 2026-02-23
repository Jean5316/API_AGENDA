using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_AGENDA.Models
{
    public class Contato
    {
        public int Id { get; set; }

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

        public bool Favorito { get; set; } = false;
        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }

        //Relacionamento com outra tabela
        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; } 
    }
}