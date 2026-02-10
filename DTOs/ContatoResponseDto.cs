using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace API_AGENDA.DTOs
{
    public class ContatoResponseDto
    {
        public int Id { get; set;}
        public string Nome { get; set;}
        public string Telefone { get; set;}
        public string? Email { get; set;}
        public string? Categoria { get; set;}
        public bool Favorito { get; set;}
        
    }
}