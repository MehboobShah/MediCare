namespace MediCare.Infrastructure.Mailing;

public class MailSettings
{
    public string? From { get; set; }

    public string? Host { get; set; }

    public int Port { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? DisplayName { get; set; }
    public string? SendGridApiKey { get; set; }
    public string? Subject { get; set; }

}