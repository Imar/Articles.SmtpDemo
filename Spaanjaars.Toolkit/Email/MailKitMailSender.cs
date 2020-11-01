using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Spaanjaars.Toolkit.Email
{
  public class MailKitMailSender : MailSenderBase
  {
    private readonly SmtpSettings _emailSettings;

    public MailKitMailSender(IOptions<List<SmtpSettings>> emailSettings)
    {
      if (!emailSettings.Value.Any())
      {
        throw new ArgumentException("Must specify at least one SMTP option.");
      }
      _emailSettings = emailSettings.Value.OrderBy(x => x.Priority).First();
    }

    public override async Task SendMessageAsync(System.Net.Mail.MailMessage mailMessage)
    {
      var message = (MimeMessage)mailMessage;

      using var client = new SmtpClient();
      await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.Port, _emailSettings.UseSsl);
      await client.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
      await client.SendAsync(message);
      await client.DisconnectAsync(true);
    }
  }
}
