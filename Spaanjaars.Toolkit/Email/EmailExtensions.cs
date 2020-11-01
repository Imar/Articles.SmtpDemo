using Microsoft.Extensions.Configuration;
using Spaanjaars.Toolkit.Email;

// ReSharper disable once CheckNamespace // Using this namespace is a best practice to bring these extensions in scope easily.
namespace Microsoft.Extensions.DependencyInjection
{
  public static class EmailExtensions
  {
    /// <summary>
    /// Adds a standard SMTP-client based MailSender.
    /// </summary>
    public static IServiceCollection AddSmtpServer(this IServiceCollection services)
    {
      services.AddSingleton<IMailSender, MailSender>();
      return services;
    }

    /// <summary>
    /// Adds a MailSender that uses MailKit.
    /// </summary>
    public static IServiceCollection AddMailKitSmtpServer(this IServiceCollection services)
    {
      services.AddSingleton<IMailSenderV2, MailKitMailSender>();
      return services;
    }

    /// <summary>
    /// Adds a MailSender that uses SendGrid.
    /// </summary>
    public static IServiceCollection AddSendGridSmtpServer(this IServiceCollection services, IConfiguration configuration)
    {
      services.Configure<SendGridSettings>(options => configuration.GetSection("SendGridSettings")
        .Bind(options, c => c.BindNonPublicProperties = true));
      services.AddSingleton<IMailSenderV2, SendGridMailSender>();
      return services;
    }

    /// <summary>
    /// Adds an InternalMailSender that always redirects mail to the specified address,
    /// </summary>
    public static IServiceCollection AddInternalSmtpServer(this IServiceCollection services, string emailAddress, IMailSender mailSender = null)
    {
      services.AddSingleton<IMailSender, InternalMailSender>(
        x => new InternalMailSender(
          emailAddress,
          mailSender));
      return services;
    }

    /// <summary>
    /// Adds an AlwaysDropMailOnLocalDiskMailSender that writes all emails to disk as .eml files.
    /// </summary>
    public static IServiceCollection AddDropLocalSmtpServer(this IServiceCollection services, string folder)
    {
      services.AddSingleton<IMailSender, AlwaysDropMailOnLocalDiskMailSender>(
        x => new AlwaysDropMailOnLocalDiskMailSender(folder));
      return services;
    }
  }
}
