using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_AGENDA.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        public bool Ativo { get; set; } = true;


        public string Name { get; set; } = string.Empty;


        public string Email { get; set; } = string.Empty;


        public string SenhaHash { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public List<Contato>? Contatos { get; set; }



    }
}