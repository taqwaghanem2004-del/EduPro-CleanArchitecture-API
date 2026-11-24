namespace EduPro.Service.Abstracts
{
    public interface IEmailsService
    {
        Task<string> SendEmail(string email, string message, string? reason);
    }
}
