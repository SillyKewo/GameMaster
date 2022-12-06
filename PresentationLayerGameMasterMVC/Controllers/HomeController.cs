using Entities;
using Microsoft.AspNetCore.Mvc;
using PresentationLayerGameMasterMVC.Models;
using System.Diagnostics;

namespace PresentationLayerGameMasterMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(GameType? gameType, DateTime? from, DateTime? to, string? requiredPlayer)
        {
            List<TournamentResult> results = TournamentCache.GetSingleton()?.TournamentResults ?? new List<TournamentResult>();

            if (gameType is not null)
            {
                results = results.Where(r => r.GameType == gameType.Value).ToList();
            }

            if (from.HasValue)
            {
                results = results.Where(r => r.TournamentHeldAt > from.Value).ToList();
            }

            if (to.HasValue)
            {
                results = results.Where(r => r.TournamentHeldAt < to.Value).ToList();
            }

            if (!string.IsNullOrEmpty(requiredPlayer))
            {
                results = results.Where(r => r.PlayerList.Select(p => p.Name).Contains(requiredPlayer)).ToList();
            }

            return View("Index", results);
        }

        public IActionResult PlayerOverview()
        {
            List<Player> players = TournamentCache.GetSingleton()?.Players ?? new List<Player>();
            return View("PlayerOverview", players);
        }

        [Route("/Player/Details")]
        public IActionResult PlayerDetails(string name)
        {
            return View("../Players/PlayerDetails", new PlayerDetailsModel(name));
        }

        [Route("/Tournament/Details")]
        public IActionResult TournamentDetails(long dateTimeTicks)
        {
            List<TournamentResult> results = TournamentCache.GetSingleton()?.TournamentResults ?? new List<TournamentResult>();
            TournamentResult x = results.Single(t => t.TournamentHeldAt.Ticks == dateTimeTicks);

            return View("../Tournament/TournamentDetails", new TournamentResultViewerModel(x));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}