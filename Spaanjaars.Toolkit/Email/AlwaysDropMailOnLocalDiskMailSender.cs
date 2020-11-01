using System.IO;
using System.Net.Mail;

namespace Spaanjaars.Toolkit.Email
{
  /// <summary>
  /// Helper class for unit tests that always writes the email file to disk.
  /// </summary>
  public class AlwaysDropMailOnLocalDiskMailSender : IMailSender
  {
    private readonly string _rootFolder;

    public AlwaysDropMailOnLocalDiskMailSender(string rootFolder)
    {
      if (!Directory.Exists(rootFolder))
      {
        Directory.CreateDirectory(rootFolder);
      }
      _rootFolder = rootFolder;
    }

    /// <summary>
    /// Sends an email as plain text.
    /// <param name="fromAddress">The email address of the sender.</param>
    /// <param name="toAddress">The email address of the recipient.</param>
    /// <param name="subject">The subject line of the email.</param>
    /// <param name="body">The body of the email as plain text.</param>
    /// </summary>
    public void SendMessage(string fromAddress, string toAddress, string subject, string body)
    {
      SendMessage(fromAddress, toAddress, subject, body, false);
    }

    /// <summary>
    /// Sends an email.
    /// </summary>
    /// <param name="fromAddress">The email address of the sender</param>
    /// <param name="toAddress">The email address of the recipient</param>
    /// <param name="subject">The subject line of the email.</param>
    /// <param name="body">The body message communication of the email.</param>
    /// <param name="isBodyHtml">Determines if the body is sent as HTML (true) or as plain text (false).</param>
    public void SendMessage(string fromAddress, string toAddress, string subject, string body, bool isBodyHtml)
    {
      var message = new MailMessage
      {
        From = new MailAddress(fromAddress),
        Subject = subject,
        Body = body,
        IsBodyHtml = isBodyHtml
      };

      message.To.Add(new MailAddress(toAddress));
      SendMessage(message);
    }

    ///<summary>
    /// Sends an email.
    /// </summary>
    /// <param name="message">The message to send.</param>
    public void SendMessage(MailMessage message)
    {
      using var smtpClient = new SmtpClient
      {
        DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
        PickupDirectoryLocation = _rootFolder
      };
      message.BodyEncoding = System.Text.Encoding.UTF8;
      smtpClient.Send(message);
    }
  }
}
