using System.Net.Mail;

namespace Spaanjaars.Toolkit.Email
{
  /// <summary>
  /// Defines an interface for sending email.
  /// </summary>
  public interface IMailSender
  {
    /// <summary>
    /// Sends an email.
    /// </summary>
    /// <param name="from">The sender's email address.</param>
    /// <param name="to">The recipient's email address.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="body">The body text of the email.</param>
    void SendMessage(string from, string to, string subject, string body);

    /// <summary>
    /// Sends an email.
    /// </summary>
    /// <param name="from">The sender's email address.</param>
    /// <param name="to">The recipient's email address.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="body">The body text of the email.</param>
    /// <param name="isBodyHtml">Determines if the body is sent as HTML or as plain text.</param>
    void SendMessage(string from, string to, string subject, string body, bool isBodyHtml);

    /// <summary>
    /// Sends an email.
    /// </summary>
    /// <param name="mailMessage">The message to send.</param>
    void SendMessage(MailMessage mailMessage);
  }
}
