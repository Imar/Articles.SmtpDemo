using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spaanjaars.Toolkit.Email;

namespace SmtpDemo.Web
{
  public class Startup
  {
    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
      _environment = environment;
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.Configure<List<SmtpSettings>>(options => Configuration.GetSection("SmtpSettings")
        .Bind(options, c => c.BindNonPublicProperties = true));

      if (_environment.IsProduction())
      {
        services.AddSmtpServer();
      }
      else
      {
        //services.AddDropLocalSmtpServer(Configuration.GetValue<string>("TempMailFolder"));
        //services.AddInternalSmtpServer("you@example.com", new MailSender());
        //services.AddSingleton<IMailSender, InternalMailSender>(
        //  x => new InternalMailSender(
        //    Configuration.GetValue<string>("DebugEmailAddress"),
        //    new AlwaysDropMailOnLocalDiskMailSender(Configuration.GetValue<string>("TempMailFolder"))));
        //services.AddMailKitSmtpServer();
        services.AddSendGridSmtpServer(Configuration);
      }
      services.AddControllersWithViews();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }
      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}
