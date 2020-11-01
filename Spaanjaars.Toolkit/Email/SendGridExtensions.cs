using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using SendGrid.Helpers.Mail;

namespace Spaanjaars.Toolkit.Email
{
  public static class SendGridExtensions
  {
    public static SendGridMessage GetSendGridMessage(this MailMessage message)
    {
      var sendGridMessage = new SendGridMessage { From = GetSendGridAddress(message.From) };
      if (message.ReplyToList.Any())
      {
        sendGridMessage.ReplyTo = message.ReplyToList.First().GetSendGridAddress();
      }

      if (message.To.Any())
      {
        var tos = message.To.Select(x => x.GetSendGridAddress()).ToList();
        sendGridMessage.AddTos(tos);
      }

      if (message.CC.Any())
      {
        var cc = message.CC.Select(x => x.GetSendGridAddress()).ToList();
        sendGridMessage.AddCcs(cc);
      }

      if (message.Bcc.Any())
      {
        var bcc = message.Bcc.Select(x => x.GetSendGridAddress()).ToList();
        sendGridMessage.AddBccs(bcc);
      }

      if (!string.IsNullOrWhiteSpace(message.Subject))
      {
        sendGridMessage.Subject = message.Subject;
      }

      if (!string.IsNullOrWhiteSpace(message.Body))
      {
        var content = message.Body;

        if (message.IsBodyHtml)
        {
          if (content.StartsWith("<html"))
          {
            content = message.Body;
          }
          else
          {
            content = $"<html><body>{message.Body}</body></html>";
          }

          sendGridMessage.AddContent("text/html", content);
        }
        else
        {
          sendGridMessage.AddContent("text/plain", content);
        }
      }

      if (message.Attachments.Any())
      {
        sendGridMessage.Attachments = new System.Collections.Generic.List<SendGrid.Helpers.Mail.Attachment>();
        sendGridMessage.Attachments.AddRange(message.Attachments.Select(GetSendGridAttachment));
      }
      return sendGridMessage;
    }

    public static EmailAddress GetSendGridAddress(this MailAddress address)
    {
      return string.IsNullOrWhiteSpace(address.DisplayName) ? new EmailAddress(address.Address) : new EmailAddress(address.Address, address.DisplayName.Replace(",", "").Replace(";", ""));
    }

    public static SendGrid.Helpers.Mail.Attachment GetSendGridAttachment(this System.Net.Mail.Attachment attachment)
    {
      using var stream = new MemoryStream();
      attachment.ContentStream.CopyTo(stream);
      return new SendGrid.Helpers.Mail.Attachment()
      {
        Disposition = "attachment",
        Type = attachment.ContentType.MediaType,
        Filename = attachment.Name,
        ContentId = attachment.ContentId,
        Content = Convert.ToBase64String(stream.ToArray())
      };
    }
  }
}