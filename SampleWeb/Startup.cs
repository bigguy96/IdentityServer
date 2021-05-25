using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using SampleWeb.Services;

namespace SampleWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ISampleClient, SampleClient>();
            services.AddHttpClient("sample", c =>
            {
                c.BaseAddress = new Uri("https://localhost:44376/");
                c.DefaultRequestHeaders.Accept.Clear();
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = "Cookies";
                opt.DefaultChallengeScheme = "oidc";
            })
             .AddCookie("Cookies")
             .AddOpenIdConnect("oidc", opt =>
              {
                  opt.SignInScheme = "Cookies";
                  opt.Authority = "https://localhost:5005";
                  opt.ClientId = "mvc-client";
                  opt.ResponseType = "code id_token";
                  opt.SaveTokens = true;
                  opt.ClientSecret = "MVCSecret";
                  opt.GetClaimsFromUserInfoEndpoint = true;
                  opt.ClaimActions.DeleteClaim("sid");
                  opt.ClaimActions.DeleteClaim("idp");
                  opt.Scope.Add("address");
                  opt.Scope.Add("roles");
                  opt.ClaimActions.MapUniqueJsonKey("role", "role");
                  opt.TokenValidationParameters = new TokenValidationParameters
                  {
                      RoleClaimType = "role"
                  };
              });
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
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
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