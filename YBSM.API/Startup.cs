using Core.Domain.Settings;
using FluentValidation.AspNetCore;
using Infrastructure.EmailService;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Web.API.ExceptionHandler;
using Web.API.Validatiors;
using YBSM.YaqeenBankSedcoMiddleware.Api.Middleware;

namespace HRM.API
{
    public class Startup
    {
        public IConfiguration configRoot 
        {
            get;
        }
        public Startup(IConfiguration configuration)
        {
            configRoot = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(_ => true));
            });

            services.AddControllers(options => { options.Filters.Add(new HttpResponseExceptionActionFilter()); });
           
            
            //services.AddFluentValidationAutoValidation();
            //services.AddFluentValidationClientsideAdapters();



            AddSwagger(services);
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
);
           

            services.Configure<JWTSettings>(configRoot.GetSection("JWTSettings"));
            services.AddSingleton<JWTSettings>();
            /* services.AddApplicationServices();
             services.AddPersistenceServices(configRoot);
             services.AddInfrastructureServices(configRoot);
             services.AddIdentityServices(configRoot);*/
            services.AdPersistenceRegistration(configRoot);
            services.AddServicesRegistration(configRoot);
            services.AddEmailServicesRegistration(configRoot);
            services.AddValidators();
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidationExceptionFilter>();
            });

            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });


            

            /* services.Configure<ApiBehaviorOptions>(options =>
             {
                 options.InvalidModelStateResponseFactory = context =>
                 {
                     var firstError = context
                         .ModelState
                         .Values
                         .SelectMany(v => v.Errors)
                         .Select((ModelError e) =>
                         {
                             // Try to extract custom error code and message from Exception.Data
                             if (e.Exception?.Data != null &&
                                 e.Exception.Data.Contains("Code") &&
                                 e.Exception.Data.Contains("Message"))
                             {
                                 return new
                                 {
                                     Code = e.Exception.Data["Code"]?.ToString() ?? "E0",
                                     Message = e.Exception.Data["Message"]?.ToString() ?? e.ErrorMessage
                                 };
                             }

                             // Default fallback
                             return new
                             {
                                 Code = "E0",
                                 Message = e.ErrorMessage
                             };
                         })
                         .FirstOrDefault();

                     return new BadRequestObjectResult(new
                     {
                         Result = new[] { firstError }
                     });
                 };
             });*/


            /* services.Configure<ApiBehaviorOptions>(options =>
             {
                 options.InvalidModelStateResponseFactory = context =>
                 {
                     var firstError = context.ModelState
                         .Values
                         .SelectMany(v => v.Errors)
                         .Select((ModelError e) =>
                         {
                             // Try to extract custom error code and message from Exception.Data
                             if (e.Exception?.Data != null &&
                                 e.Exception.Data.Contains("Code") &&
                                 e.Exception.Data.Contains("Message"))
                             {
                                 return new
                                 {
                                     Code = e.Exception.Data["Code"]?.ToString() ?? "E0",
                                     Message = e.Exception.Data["Message"]?.ToString() ?? e.ErrorMessage
                                 };
                             }

                             // Default fallback
                             return new
                             {
                                 Code = "E0",
                                 Message = e.ErrorMessage
                             };
                         })
                         .FirstOrDefault();

                     return new BadRequestObjectResult(new
                     {
                         Result = new[] { firstError }
                     });
                 };
             });*/


        }



        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,

                        },
                        new List<string>()
                      }
                    });

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "YBSM Yaqeen Bank Sedco Middleware API",

                });
            });
        }
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseCustomExceptionHandler();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "YBSM Yaqeen Bank Sedco Middleware API");
            });
            app.UseCors("Open");
            app.UseEndpoints(endpoint => endpoint.MapControllers());
            app.Run();

        }
    }
}
