using System.Net.Mail;
using System.Threading.Tasks;

namespace Spaanjaars.Toolkit.Email
{
  /// <summary>
  /// Defines an interface for sending email.
  /// </summary>
  public interface IMailSenderV2
  {
    /// <summary>
    /// Sends an email.
    /// </summary>
    /// <param name="from">The sender's email address.</param>
    /// <param name="to">The recipient's email address.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="body">The body text of the email.</param>
    Task SendMessageAsync(string from, string to, string subject, string body);

    /// <summary>
    /// Sends an email.
    /// </summary>
    /// <param name="from">The sender's email address.</param>
    /// <param name="to">The recipient's email address.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="body">The body text of the email.</param>
    /// <param name="isBodyHtml">Determines if the body is sent as HTML or as plain text.</param>
    Task SendMessageAsync(string from, string to, string subject, string body, bool isBodyHtml);

    /// <summary>
    /// Sends an email.
    /// </summary>
    /// <param name="mailMessage">The message to send.</param>
    Task SendMessageAsync(MailMessage mailMessage);
  }
}
