namespace TradeSphere.Application.Contracts.DTOs.AuthDto
{
    public class ConfirmChangeEmailRequest
    {
        public string NewEmail { get; set; }
        public string Token { get; set; }
    }
}
