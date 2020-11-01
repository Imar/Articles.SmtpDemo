using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Spaanjaars.Toolkit.Email;

namespace SmtpDemo.Web.Controllers
{
  public class HomeController : ControllerBase
  {
    private readonly IMailSenderV2 _mailSender;

    public HomeController(IMailSenderV2 mailSender)
    {
      _mailSender = mailSender;
    }

    public async Task<ActionResult> SendIt()
    {
      try
      {
        await _mailSender.SendMessageAsync("sender@example.com", "recipient@example.com", "Your account confirmation", BuildBody());
        return Ok();
      }
      catch
      {
        // Todo log error
        return StatusCode(500);
      }
    }

    private string BuildBody()
    {
      return "Hello world";
    }
  }
}
