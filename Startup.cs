using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookchin.Library.API.Data;
using Bookchin.Library.API.Data.Contexts;
using Bookchin.Library.API.Data.Models;
using Bookchin.Library.API.Middleware;
using Bookchin.Library.API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Bookchin.Library.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Database related services
            services.AddDbContext<AppDbContext>();

            services.AddScoped<UserAccountsRepository>();
            services.AddScoped<IndividualsRepository>();
            services.AddScoped<OrganizationsRepository>();

            // Identity related services
            services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    string jwtIssuer = this.Configuration.GetValue<string>("Jwt:Issuer");
                    string jwtSecret = this.Configuration.GetValue<string>("Jwt:Secret");

                    var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = jwtIssuer,
                        ValidIssuer = jwtIssuer,
                        IssuerSigningKey = issuerSigningKey
                    };
                });

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Bookchin.Library.API",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme 
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    } 
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(ApplicationExceptionHandler.Invoke);

            if (env.IsDevelopment())
            {
                var serviceProvider = app.ApplicationServices
                    .GetRequiredService<IServiceScopeFactory>()
                    .CreateScope()
                    .ServiceProvider;

                var context = serviceProvider
                    .GetRequiredService<AppDbContext>();

                var userManager = serviceProvider
                    .GetRequiredService<UserManager<ApplicationUser>>();

                var databaseSeeder = new DatabaseSeeder(context, userManager);

                databaseSeeder.Initialize();

                // app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bookchin.Library.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
