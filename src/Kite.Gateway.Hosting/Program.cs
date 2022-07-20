using Kite.Gateway.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host
     .ConfigureLogging((context, logBuilder) =>
     {
         Log.Logger = new LoggerConfiguration()
          .Enrich.FromLogContext()
          .WriteTo.Console()// ��־���������̨
          .MinimumLevel.Information()
          .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
          .CreateLogger();
         logBuilder.ClearProviders();
         logBuilder.AddSerilog(dispose: true);
     })
     .UseAutofac();


//�����������С
builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.Limits.MaxRequestBodySize = int.MaxValue;
});
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = int.MaxValue;
});
builder.Services.ReplaceConfiguration(builder.Configuration);//�������ô���

builder.Services.AddApplication<HostingModule>();
var app = builder.Build();
app.InitializeApplication();
app.MapGet("/", context => context.Response.WriteAsync("hello world!!!"));
app.Run();
