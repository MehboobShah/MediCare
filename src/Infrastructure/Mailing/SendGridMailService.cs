using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using MediCare.Application.Common.Mailing;

namespace MediCare.Infrastructure.Mailing;
public class SendGridMailService : IMailService
{
    private readonly MailSettings _settings;
    private readonly ILogger<SendGridMailService> _logger;

    public SendGridMailService(IOptions<MailSettings> settings, ILogger<SendGridMailService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task SendAsync(MailRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var client = new SendGridClient(_settings.SendGridApiKey);
            var email = new SendGridMessage()
            {
                From = new EmailAddress(request.From ?? _settings.From, _settings.DisplayName),
                Subject = request.Subject ?? _settings.Subject,
                HtmlContent = request.Body
            };

            // To
            List<EmailAddress> emails = new List<EmailAddress>();
            foreach (string address in request.To)
                emails.Add(new EmailAddress(address));
            email.AddTos(emails);

            await client.SendEmailAsync(email).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
