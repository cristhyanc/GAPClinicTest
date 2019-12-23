using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAPClinicTest.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using GAPClinicTest.Core.Interfaces;
using GAPClinicTest.Core.UseCases;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Net.Http.Headers;

namespace GAPClinicTest.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(MyAllowSpecificOrigins,
            //    builder =>
            //    {
            //        builder.WithOrigins("https://localhost:44335").AllowAnyHeader().AllowCredentials()
            //        .AllowCredentials().WithHeaders(HeaderNames.ContentType, HeaderNames.Authorization, "x-custom-header")
            //                    .AllowAnyMethod();
            //    });
            //});
            services.AddCors();
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(MyAllowSpecificOrigins,
            //        builder => builder.AllowAnyOrigin()
            //        .AllowAnyMethod()
            //        .AllowAnyHeader());
            //});

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GAPClinicAPI", Version = "v1" });
            });

            services.AddDbContext<ClinicContext>(options =>
           options.UseSqlServer(Configuration.GetConnectionString("ClinicContext")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPatientUseCase, PatientUseCase>();
            services.AddScoped<IAppointmentUserCase, AppointmentUserCase>();


            var key = Encoding.ASCII.GetBytes("9ecc0154-2a82-4633-a567-db9b742c32b4");

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
          .AddJwtBearer(x =>
          {
              x.Events = new JwtBearerEvents
              {
                  OnTokenValidated = context =>
                  {
                      var userId = context.Principal.Identity.Name;
                      if (string.IsNullOrEmpty(userId))
                      {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                      }
                      return Task.CompletedTask;
                  }
              };
            
              x.RequireHttpsMetadata = false;
              x.SaveToken = true;
              x.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = new SymmetricSecurityKey(key),
                  ValidateIssuer = true,
                  ValidateAudience = true,
                  ValidIssuer = "http://localhost:44345/",
                  ValidAudience = "http://localhost:44345/",
              };
          });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseRouting();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GAPClinicAPI V1");
               
            });

            app.UseAuthorization();
            //app.UseCors(MyAllowSpecificOrigins);
            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
