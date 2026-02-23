namespace API_AGENDA.DTOs
{
    public class LoginResponseDto
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }

    }
}
