using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Usuario
{
    public class UsuarioResponseDto
    {
        public int Id { get; set; }

        public string? Nome { get; set; }

        public string? Email { get; set; }

        public string? Role { get; set; }
        public bool Ativo { get; set; }



    }
}