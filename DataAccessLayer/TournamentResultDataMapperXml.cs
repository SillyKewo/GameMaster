using Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace GameMaster.DataAccessLayer
{
    public class TournamentResultDataMapperXml
    {
        private string _outputFolder;

        private const string Prefix = "_results.xml";
        private XmlSerializer _serializer;

        public TournamentResultDataMapperXml(string outputFolder)
        {
            this._outputFolder = outputFolder;
            this._serializer = new XmlSerializer(typeof(List<TournamentResultDTO>));
        }


        public void SaveTournamentsForDate(List<TournamentResult> results, DateTime date)
        {
            string outFileName = date.Date.ToShortDateString() + Prefix;

            List<TournamentResultDTO> dateResults;

            if (File.Exists(this._outputFolder + outFileName))
            {
                using (var reader = XmlReader.Create(this._outputFolder + outFileName))
                {
                    dateResults = (List<TournamentResultDTO>?)this._serializer.Deserialize(reader) ?? new List<TournamentResultDTO>();
                    
                }
            }
            else
            {
                dateResults = new List<TournamentResultDTO>();
            }

            dateResults.AddRange(results.Select(r => this.ConvertToDTO(r)));
            var settings = new XmlWriterSettings() { Indent = true, NewLineOnAttributes = true };
            using (var writer = XmlWriter.Create(this._outputFolder + outFileName, settings))
            {
                this._serializer.Serialize(writer, dateResults);
            }

        }

        public bool TryGetTournamentsForDate(DateTime date, [NotNullWhen(true)] out List<TournamentResult>? tournamentResults)
        {
            tournamentResults = null;
            string outFileName = date.Date.ToShortDateString() + Prefix;

            using (var reader = XmlReader.Create(this._outputFolder + outFileName))
            {
                var tournamentResultsDTOs = (List<TournamentResultDTO>?)this._serializer.Deserialize(reader);

                if (tournamentResultsDTOs != null && tournamentResultsDTOs.Any())
                {
                    tournamentResults = tournamentResultsDTOs.Select(t => this.ConvertFromDTO(t)).ToList();
                }
            }

            return tournamentResults != null;
        }

        public List<TournamentResult> GetAllTournamentResults()
        {
            List<TournamentResult> results = new List<TournamentResult>();

            string[] files = Directory.GetFiles(this._outputFolder);

            foreach (string filePath in files)
            {
                using (var reader = XmlReader.Create(filePath))
                {
                    var tournamentResultsDTOs = (List<TournamentResultDTO>?)this._serializer.Deserialize(reader);

                    if (tournamentResultsDTOs != null && tournamentResultsDTOs.Any())
                    {
                        results.AddRange(tournamentResultsDTOs.Select(t => this.ConvertFromDTO(t)).ToList());
                    }
                }
            }

            return results;
        }

        private TournamentResultDTO ConvertToDTO(TournamentResult tournamentResult)
        {
            List<MatchResultDTO> matchResultDTOs = new List<MatchResultDTO>();
            foreach(var matchResult in tournamentResult.MatchResults)
            {
                List<GameResultDTO> gameResultDTOs = new List<GameResultDTO>();
                foreach(var gameResult in matchResult.GameResults)
                {
                    List<MoveDTO> moveDTO = gameResult.Moves.Select(m => new MoveDTO { Commands = m.Commands, Player = m.Player.Name }).ToList();

                    gameResultDTOs.Add(new GameResultDTO
                    {
                        Moves = moveDTO,
                        HasWinner = gameResult.HasWinner,
                        ConditionPlayer = gameResult.ResultConditionPlayer?.Name,
                        Players = gameResult.Players.Select(p => p.Name).ToList(),
                        TimedOut = gameResult.GameResultCondition == GameResult.ResultCondition.TimeOut,
                        Scores = gameResult.Scores
                    });

                }

                matchResultDTOs.Add(new MatchResultDTO
                {
                    GameResults = gameResultDTOs,
                    Players = matchResult.GameResults.SelectMany(gr => gr.Players).Distinct().Select(p => p.Name).ToList(),
                    GameBoardConfig = ConvertToDTO(matchResult.GameBoardConfiguration)
                });
            }

            return new TournamentResultDTO
            {
                DateTime = tournamentResult.TournamentHeldAt,
                MatchResults = matchResultDTOs,
                GameType = tournamentResult.GameType,
                VersusMode = tournamentResult.VersusMode
            };
        }

        private GameBoardConfigDTO ConvertToDTO(GameBoardConfiguration gameBoardConfiguration)
        {
            switch (gameBoardConfiguration)
            {
                case TicTacToeGameBoardConfiguration ticTacToeConfiguration:
                    return new TicTacToeGameBoardDTO
                    {
                        BoardSize = ticTacToeConfiguration.BoardSize
                    };

                default:
                    throw new NotImplementedException();
            }
        }

        private GameBoardConfiguration ConvertFromDTO(GameBoardConfigDTO gameBoardConfiguration)
        {
            switch (gameBoardConfiguration)
            {
                case TicTacToeGameBoardDTO ticTacToeConfiguration:
                    return new TicTacToeGameBoardConfiguration
                    {
                        BoardSize = ticTacToeConfiguration.BoardSize
                    };

                default:
                    throw new NotImplementedException();
            }
        }

        private TournamentResult ConvertFromDTO(TournamentResultDTO tournamentResultDTO)
        {
            List<MatchResult> matchResults = new List<MatchResult>();
            foreach (var matchResultDTO in tournamentResultDTO.MatchResults)
            {
                List<GameResult> gameResults = new List<GameResult>();
                foreach (var gameResultDto in matchResultDTO.GameResults)
                {
                    var players = gameResultDto.Players.Select(p => new Player(p)).ToList();
                    var moves = gameResultDto.Moves.Select(m => new Move(m.Commands, players.First(p => p.Name == m.Player))).ToList();
                    var gameResult = gameResultDto switch
                    {
                        _ when gameResultDto.TimedOut && gameResultDto.ConditionPlayer is not null => GameResult.CreateTimedOutResult(players, moves, players.First(p => p.Name == gameResultDto.ConditionPlayer), gameResultDto.Scores),
                        _ when gameResultDto.HasWinner && gameResultDto.ConditionPlayer is not null => GameResult.CreateResult(players, moves, players.First(p => p.Name == gameResultDto.ConditionPlayer), gameResultDto.Scores),
                        _ when !gameResultDto.HasWinner => GameResult.CreateDrawResult(players, moves, gameResultDto.Scores),
                        _ => throw new InvalidOperationException("Unexpected game result DTO")
                    };
                    
                    gameResults.Add(gameResult);

                }

                matchResults.Add(new MatchResult(gameResults, ConvertFromDTO(matchResultDTO.GameBoardConfig)));
            }

            return new TournamentResult(matchResults, tournamentResultDTO.DateTime, tournamentResultDTO.GameType, tournamentResultDTO.VersusMode);
        }
    }
}
