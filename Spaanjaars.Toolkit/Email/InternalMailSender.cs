using System.Net.Mail;
using System.Text;

namespace Spaanjaars.Toolkit.Email
{
  public class InternalMailSender : IMailSender
  {
    private readonly string _debugAddress;
    private readonly IMailSender _mailSender;

    public InternalMailSender(string debugAddress, IMailSender mailSender)
    {
      _debugAddress = debugAddress;
      _mailSender = mailSender;
    }

    public void SendMessage(string from, string to, string subject, string body)
    {
      SendMessage(from, to, subject, body, false);
    }

    public void SendMessage(string from, string to, string subject, string body, bool isBodyHtml)
    {
      var mailMessage = new MailMessage { From = new MailAddress(from), Subject = subject, Body = body, IsBodyHtml = isBodyHtml };
      mailMessage.To.Add(new MailAddress(to, "Hello"));
      SendMessage(mailMessage);
    }

    public void SendMessage(MailMessage mailMessage)
    {
      var sb = new StringBuilder();
      var lineBreak = mailMessage.IsBodyHtml ? "<br />" : "\r\n";
      foreach (var mailAddress in mailMessage.To)
      {
        sb.Append(BuildLine(nameof(mailMessage.To), mailAddress, lineBreak));
      }
      foreach (var mailAddress in mailMessage.CC)
      {
        sb.Append(BuildLine(nameof(mailMessage.CC), mailAddress, lineBreak));
      }
      foreach (var mailAddress in mailMessage.Bcc)
      {
        sb.Append(BuildLine(nameof(mailMessage.Bcc), mailAddress, lineBreak));
      }

      sb.Append(mailMessage.Body);
      mailMessage.Body = sb.ToString();

      mailMessage.To.Clear();
      mailMessage.CC.Clear();
      mailMessage.Bcc.Clear();
      mailMessage.To.Add(_debugAddress);

      mailMessage.BodyEncoding = Encoding.UTF8;
      _mailSender.SendMessage(mailMessage);
    }

    private string BuildLine(string collectionName, MailAddress address, string lineBreak)
    {
      var result = $"Message ({collectionName}) addressed to {address.Address}";
      if (!string.IsNullOrWhiteSpace(address.DisplayName))
      {
        result += $" ({address.DisplayName})";
      }
      return result + lineBreak;
    }
  }
}
