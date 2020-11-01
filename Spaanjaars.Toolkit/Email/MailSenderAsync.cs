using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Spaanjaars.Toolkit.Email
{
  /// <summary>
  /// Sends emails using the SmtpClient.
  /// </summary>
  public class MailSenderAsync : IMailSenderV2
  {
    private readonly SmtpSettings[] _emailSettings;

    public MailSenderAsync(IOptions<List<SmtpSettings>> emailSettings)
    {
      if (!emailSettings.Value.Any())
      {
        throw new ArgumentException("Must specify at least one SMTP option.");
      }
      _emailSettings = emailSettings.Value.OrderBy(x => x.Priority).ToArray();
    }

    /// <summary>
    /// Sends an email as plain text.
    /// </summary>
    /// <param name="from">The sender's email address.</param>
    /// <param name="to">The recipient's email address.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="body">The body text of the email.</param>
    public async Task SendMessageAsync(string from, string to, string subject, string body)
    {
      await SendMessageAsync(from, to, subject, body, false);
    }

    /// <summary>
    /// Sends an email.
    /// </summary>
    /// <param name="from">The sender's email address.</param>
    /// <param name="to">The recipient's email address.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="body">The body text of the email.</param>
    /// <param name="isBodyHtml">Determines if the body is sent as HTML or as plain text.</param>
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

    /// <summary>
    /// Sends an email.
    /// </summary>
    /// <param name="mailMessage">The message to send.</param>
    public async Task SendMessageAsync(MailMessage mailMessage)
    {
      mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
      bool shouldTrySendEmail;
      var count = 0;
      do
      {
        shouldTrySendEmail = !await SendMailAsync(_emailSettings[count], mailMessage);
        count++;
      } while (shouldTrySendEmail && count < _emailSettings.Length);
    }

    private async Task<bool> SendMailAsync(SmtpSettings smtpSettings, MailMessage mailMessage)
    {
      var client = BuildClient(smtpSettings);
      try
      {
        await client.SendMailAsync(mailMessage);
        return true;
      }
      catch (SmtpException smtpEx)
      {
        switch (smtpEx.StatusCode)
        {
          case SmtpStatusCode.MailboxBusy: // Sleep for a bit and then retry. // If it fails again, try the alternative server.
            return await RetryAsync(mailMessage, client);

          // Ignore the status codes below. Resending using an alternate email server won't help in these case.
          case SmtpStatusCode.InsufficientStorage:
          case SmtpStatusCode.MailboxNameNotAllowed:
          case SmtpStatusCode.ExceededStorageAllocation:
          case SmtpStatusCode.CannotVerifyUserWillAttemptDelivery:
          case SmtpStatusCode.UserNotLocalWillForward:
            return true;

          default:
            return false; // Attempt to resend with an alternative server on other status codes.
        }
      }
      catch (Exception)
      {
        return true; // Don't retry on other exceptions
      }
    }

    private static async Task<bool> RetryAsync(MailMessage mailMessage, SmtpClient client)
    {
      System.Threading.Thread.Sleep(2000);
      try
      {
        await client.SendMailAsync(mailMessage);
        return true;
      }
      catch (SmtpException)
      {
        // Failed twice now; let's retry the next server
        return false;
      }
    }

    private static SmtpClient BuildClient(SmtpSettings smtpSettings)
    {
      return new SmtpClient(smtpSettings.MailServer, smtpSettings.Port)
      {
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(smtpSettings.UserName, smtpSettings.Password),
        EnableSsl = true
      };
    }
  }
}
