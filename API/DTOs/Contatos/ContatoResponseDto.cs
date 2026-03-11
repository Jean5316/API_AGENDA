using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API_AGENDA.Models;

namespace API_AGENDA.DTOs
{
    public class ContatoResponseDto
    {
        public int Id { get; set;}
        public string Nome { get; set;} = string.Empty;
        public string Telefone { get; set;} = string.Empty;
        public string Email { get; set;} = string.Empty;
        public EnumCategorias Categoria { get; set;}
        public bool Favorito { get; set;}
        
    }
}