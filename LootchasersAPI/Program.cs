
namespace LootchasersAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddHttpClient();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Lootchasers Clan API");
            });

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/")
                {
                    context.Response.Redirect("/swagger");
                }
                else
                {
                    await next();
                }
            });

            app.MapGet("/discord", (HttpContext context) =>
            {
                context.Response.Redirect("https://discord.gg/lootchasers");
                return Task.CompletedTask;
            });
            app.MapControllers();

            app.Run();
        }
    }
}
