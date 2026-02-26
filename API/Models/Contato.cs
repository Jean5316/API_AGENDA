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


        public string Nome { get; set; } = string.Empty;


        public string Telefone { get; set; } = string.Empty;


        public string Email { get; set; } = string.Empty;


        public string Categoria { get; set; } = string.Empty;

        public bool Favorito { get; set; } = false;
        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }

        //Relacionamento com outra tabela
        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; }
    }
}