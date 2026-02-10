using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_AGENDA.Models
{
    public class Contato
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)] 
        public string Nome { get; set; }

        [Required]
        [Phone]
        [MaxLength(20)]
        public string Telefone { get; set; }

        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Categoria { get; set; }

        public bool Favorito { get; set; } = false;
        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }
    }
}