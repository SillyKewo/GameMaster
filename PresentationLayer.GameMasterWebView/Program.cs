using PresentationLayer.GameMasterWebView;

public class Program
{
    private static TournamentCache _cache;
    private const string loadFolder = @"C:\Users\theke\Documents\GameMaster\Output\";
    private static void Main(string[] args)
    {
        _cache = TournamentCache.Create(new GameMaster.DataAccessLayer.TournamentResultDataMapperXml(loadFolder));
        _cache.StartCacheRefresh(TimeSpan.FromSeconds(5));

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);



        // Add services to the container.
        builder.Services.AddRazorPages();

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();
    }
}