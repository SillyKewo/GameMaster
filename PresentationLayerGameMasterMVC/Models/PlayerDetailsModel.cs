using Entities;

namespace PresentationLayerGameMasterMVC.Models
{
    public class PlayerDetailsModel
    {
        public Player Player { get; }

        public PlayerStatistics OverallStatics { get; }

        public List<(GameType, PlayerStatistics)> GameTypeStatistics { get; }

        public List<TournamentResult> LatestTournaments { get; }

        public PlayerDetailsModel(string name)
        {
            this.Player = TournamentCache.GetSingleton()?.Players.Single(p => p.Name == name) ?? new Player("Unknown");
            var results = TournamentCache.GetSingleton()?.TournamentResults.Where(t => t.PlayerList.Contains(this.Player)).ToList() ?? new List<TournamentResult>();
            this.OverallStatics = new PlayerStatistics(this.Player, results);

            List<(GameType, PlayerStatistics)> gameStatistics = new List<(GameType, PlayerStatistics)>();

            foreach (var item in results.GroupBy(t => t.GameType))
            {
                gameStatistics.Add((item.Key, new PlayerStatistics(this.Player, item.ToList())));
            }

            this.GameTypeStatistics = gameStatistics;
            this.LatestTournaments = results
                .OrderByDescending(t => t.TournamentHeldAt)
                .Take(20).ToList();
        }

        public class PlayerStatistics
        {
            public PlayerStatistics(Player player, List<TournamentResult> tournamentResults)
            {
                List<MatchResult> matches = tournamentResults.SelectMany(t => t.MatchResults).Where(m => m.Players.Contains(player)).ToList();

                this.TournamentParticipatedIn = tournamentResults.Count;
                this.MatchesParticipatedIn = matches.Count;
                this.MatchWinRatioPercentage = matches.Count == 0 ? 0.0 : ((double)matches.Where(m => player == m.Winner).Count() / matches.Count) * 100;
                this.DrawRatioPercentage = matches.Count == 0 ? 0.0 : ((double)matches.Sum(m => m.IsDraw ? 1 : 0) / matches.Count) * 100;
            }

            public int TournamentParticipatedIn { get; }

            public double MatchWinRatioPercentage { get; }

            public double DrawRatioPercentage { get; }

            public int MatchesParticipatedIn { get; }
        }
    }
}
