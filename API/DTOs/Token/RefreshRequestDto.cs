using System.ComponentModel;

namespace API_AGENDA.DTOs
{
    public class RefreshRequestDto
    {
        [DefaultValue("string")]
        public string? RefreshToken { get; set; }
    }
}
