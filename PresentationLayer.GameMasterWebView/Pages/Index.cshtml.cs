using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PresentationLayer.GameMasterWebView.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<TournamentResult> TournamentResults { get; private set; }


        public IndexModel(ILogger<IndexModel> logger)
        {
            this.TournamentResults = TournamentCache.GetSingleton()?.TournamentResults ?? new List<TournamentResult>();
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}