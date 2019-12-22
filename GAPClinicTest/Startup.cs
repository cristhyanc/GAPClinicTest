using GAPClinicTest.Rest;
using GAPClinicTest.Services;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Syncfusion.EJ2.Blazor;

namespace GAPClinicTest
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            string url = "https://localhost:44345";
            services.AddSyncfusionBlazor();            
            services.AddScoped<PatientService>(x => new PatientService(Shared.AppSettings.APITOKEN , url));
            services.AddScoped<AppointmentService>(x => new AppointmentService(Shared.AppSettings.APITOKEN, url));
            services.AddScoped<LoginService>(x => new LoginService( url));
            
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
