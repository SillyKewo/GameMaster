// See https://aka.ms/new-console-template for more information
using Entities;
using GameMaster.DataAccessLayer;
using System.Globalization;

public static class Program
{
    public static void Main(string[] args)
    {
        string loadFolder = @"C:\Users\theke\Documents\GameMaster\Output\";
        TournamentResultDataMapperXml dataMapper = new TournamentResultDataMapperXml(loadFolder);

        List<TournamentResult> tournamentResults = dataMapper.GetAllTournamentResults().OrderByDescending(t => t.TournamentHeldAt).ToList();

        for (int i = 0; i < tournamentResults.Count; i++)
        {
            TournamentResult tournament = tournamentResults[i];
            Console.WriteLine($"[{i}]: {tournament.TournamentOverviewDescription()}");
        }

        Console.WriteLine($"Write index to get specific match results for tournament");
        Console.WriteLine();
        int index = int.Parse(Console.ReadLine() ?? "0", CultureInfo.InvariantCulture);

        TournamentResult chosenTournament = tournamentResults[index];

        foreach (var matchResult in chosenTournament.MatchResults)
        {
            Console.WriteLine(matchResult.MatchResultDescription());
            Console.WriteLine("!----------------------------------------------------------------------------!");
            Console.WriteLine();

        }
    }
}

