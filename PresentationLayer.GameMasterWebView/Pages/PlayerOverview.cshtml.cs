using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PresentationLayer.GameMasterWebView.Pages
{
    public class PlayerOverviewModel : PageModel
    {
        private readonly ILogger<PlayerOverviewModel> _logger;
        public List<Player> Players { get; private set; }


        public PlayerOverviewModel(ILogger<PlayerOverviewModel> logger)
        {
            this.Players = TournamentCache.GetSingleton()?.Players ?? new List<Player>();
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}