using System.Net.Mail;
using System.Threading.Tasks;

namespace Spaanjaars.Toolkit.Email
{
  public abstract class MailSenderBase : IMailSenderV2
  {
    public async Task SendMessageAsync(string from, string to, string subject, string body)
    {
      await SendMessageAsync(from, to, subject, body, true);
    }

    public async Task SendMessageAsync(string from, string to, string subject, string body, bool isBodyHtml)
    {
      var mailMessage = new MailMessage
      {
        From = new MailAddress(from),
        Subject = subject,
        Body = body,
        IsBodyHtml = isBodyHtml
      };
      mailMessage.To.Add(new MailAddress(to));
      await SendMessageAsync(mailMessage);
    }

    public abstract Task SendMessageAsync(MailMessage mailMessage);
  }
}