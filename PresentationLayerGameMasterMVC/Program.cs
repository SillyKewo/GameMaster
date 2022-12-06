using PresentationLayerGameMasterMVC;

internal class Program
{
    private static TournamentCache _cache;
    private const string loadFolder = @"C:\Users\theke\Documents\GameMaster\Output\";
    private static void Main(string[] args)
    {
        _cache = TournamentCache.Create(new GameMaster.DataAccessLayer.TournamentResultDataMapperXml(loadFolder));
        _cache.StartCacheRefresh(TimeSpan.FromHours(2));

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}