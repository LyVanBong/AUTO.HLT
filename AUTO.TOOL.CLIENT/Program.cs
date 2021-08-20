using AUTO.TOOL.CLIENT.Services.Database;
using AUTO.TOOL.CLIENT.Services.Facebook;
using AUTO.TOOL.CLIENT.Services.LicenseKey;
using AUTO.TOOL.CLIENT.Services.Login;
using AUTO.TOOL.CLIENT.Services.RequestProvider;
using AUTO.TOOL.CLIENT.Services.RestSharp;
using AUTO.TOOL.CLIENT.Services.Telegram;
using AUTO.TOOL.CLIENT.Services.User;
using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AUTO.TOOL.CLIENT
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services
                .AddBlazoredLocalStorage()
                .AddBlazoredToast()
                .AddSingleton<IDatabaseService, DatabaseService>()
                .AddScoped<IFacebookService, FacebookeService>()
                .AddScoped<ILicenseKeyService, LicenseKeyService>()
                .AddScoped<ILoginService, LoginService>()
                .AddScoped<IRequestProvider, RequestProvider>()
                .AddScoped<IRestSharpService, RestSharpService>()
                .AddScoped<ITelegramService, TelegramService>()
                .AddScoped<IUserService, UserService>()
                .AddScoped(client => new HttpClient()
                {
                    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
                })
                .AddHttpClient("fb", client => new HttpClient { BaseAddress = new Uri("https://m.facebook.com/") });

            //var host = builder.Build();
            //var data = host.Services.GetRequiredService<IDatabaseService>();
            //await data.GetAccountUser();

            await builder.Build().RunAsync();
        }
    }
}