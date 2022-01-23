using GameMaster.Entities;
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
            string outFileName = date.Date.ToOADate() + Prefix;

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

            using (var writer = XmlWriter.Create(this._outputFolder + outFileName))
            {
                this._serializer.Serialize(writer, dateResults);
            }

        }

        public bool TryGetTournamentsForDate(DateTime date, [NotNullWhen(true)] out List<TournamentResult>? tournamentResults)
        {
            tournamentResults = null;
            string outFileName = date.Date.ToShortTimeString() + Prefix;

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
                        Player = gameResult.ResultConditionPlayer?.Name,
                        TimedOut = gameResult.GameResultCondition == GameResult.ResultCondition.TimeOut
                    });

                }

                matchResultDTOs.Add(new MatchResultDTO
                {
                    GameResults = gameResultDTOs,
                    Players = matchResult.GameResults.SelectMany(gr => gr.Players).Distinct().Select(p => p.Name).ToList()
                });
            }

            return new TournamentResultDTO
            {
                DateTime = tournamentResult.TournamentHeldAt,
                MatchResults = matchResultDTOs,
                GameType = tournamentResult.GameType
            };
        }

        private TournamentResult ConvertFromDTO(TournamentResultDTO tournamentResultDTO)
        {
            throw new NotImplementedException();
        }
    }
}
