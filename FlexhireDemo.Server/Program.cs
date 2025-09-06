namespace FlexhireDemo.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var corsPolicyName = "AllowFlexhireDemoClient";

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: corsPolicyName,
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200", 
                                           "https://localhost:4200")
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });
            builder.Services.AddSingleton<FlexhireApiKeyProvider>();
            builder.Services.AddSingleton<GraphQLService>();
            builder.Services.AddControllers();

            var app = builder.Build();

            // Enable CORS
            app.UseCors(corsPolicyName);

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
