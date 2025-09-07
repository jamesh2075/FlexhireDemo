using FlexhireDemo.Server.Models;
using Microsoft.OpenApi.Models;

namespace FlexhireDemo.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var corsPolicyName = "AllowFlexhireDemoClient";

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSignalR();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: corsPolicyName,
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200", 
                                           "https://localhost:4200")
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                    });
            });
            builder.Services.AddSingleton<FlexhireApiKeyProvider>();
            builder.Services.AddSingleton<GraphQLService>();
            builder.Services.AddControllers();

            // Tell ASP.NET Core to add the services that handle OpenAPI (Swagger) requests
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen((c) =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = $"Flexhire API - {builder.Environment.EnvironmentName}",
                    Description = "An API for providing a subset of the Flexhire GraphQL API",
                    Version = "v1",
                    Contact = new OpenApiContact()
                    {
                        Name = "James Henry",
                        Url = new Uri("https://github.com/jamesh2075/FlexhireDemo")
                    }
                });
                c.DocInclusionPredicate((name, api) => api.HttpMethod != null);

                var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            var app = builder.Build();

            // This allows users to browse to the JSON-generated OpenAPI document
            app.UseSwagger();

            // This allows users to browse to the HTML-generated OpenAPI document
            app.UseSwaggerUI();

            // Enable CORS
            app.UseCors(corsPolicyName);

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllers();

            app.MapHub<WebhookHub>("/webhookHub");

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
