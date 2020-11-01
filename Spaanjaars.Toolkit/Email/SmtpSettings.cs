namespace Spaanjaars.Toolkit.Email
{
  public class SmtpSettings
  {
    public string MailServer { get; private set; }
    public int Port { get; private set; }
    public string UserName { get; private set; }
    public string Password { get; private set; }
    public bool UseSsl { get; private set; }
    public int Priority { get; private set; }
  }
}