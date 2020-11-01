using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid;

namespace Spaanjaars.Toolkit.Email
{
  public class SendGridMailSender : MailSenderBase
  {
    private readonly string _apiKey;

    public SendGridMailSender(IOptions<SendGridSettings> sendGridSettings)
    {
      _apiKey = sendGridSettings.Value.ApiKey;
    }

    public override async Task SendMessageAsync(MailMessage mailMessage)
    {
      var client = new SendGridClient(_apiKey);
      var msg = mailMessage.GetSendGridMessage();
//      msg.CustomArgs = new Dictionary<string, string> { { "OrderId", "1234" } };
      await client.SendEmailAsync(msg);
    }
  }
}
