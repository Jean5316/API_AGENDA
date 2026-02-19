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
        public string Nome { get; set;} = string.Empty;
        public string Telefone { get; set;} = string.Empty;
        public string Email { get; set;} = string.Empty;
        public string Categoria { get; set;} = string.Empty;
        public bool Favorito { get; set;}
        
    }
}