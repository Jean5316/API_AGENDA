using API_AGENDA.Models;

namespace API_AGENDA.Services.Interfaces
{
    public interface ItokenService
    {
        public string CreateToken(Usuario usuario);
        public string refreshToken();
    }
}
