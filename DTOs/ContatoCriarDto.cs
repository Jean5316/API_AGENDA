using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_AGENDA.DTOs
{
    public class ContatoCriarDto
    {
        [Required]
        public string Nome { get; set;}

        [Required]
        public string Telefone { get; set;}

        public string? Email { get; set;}
        public string? Categoria { get; set;}
        public bool Favorito { get; set;}
        
    }
}