using Entities;
using System.Runtime.CompilerServices;
using System.Text;

namespace PresentationLayerGameMasterMVC.Models
{
    public class TournamentResultViewerModel
    {
        public TournamentResultViewerModel(TournamentResult tournamentResult)
        {
            this.TournamentResult = tournamentResult;
            this.MatchResults = tournamentResult.MatchResults.Select(m => new MatchResultViewerModel(m)).ToList();
        }

        public TournamentResult TournamentResult { get; }

        public List<MatchResultViewerModel> MatchResults { get; }
    }


    public class MatchResultViewerModel
    {
        public MatchResultViewerModel(MatchResult matchResult)
        {
            this.MatchResult = matchResult;
            this.GameResults = matchResult.GameResults.Select(g => new GameResultViewerModel(g, matchResult.GameBoardConfiguration, true)).ToList();
        }

        public MatchResult MatchResult { get; }

        public List<GameResultViewerModel> GameResults { get; }
    }

    public class GameResultViewerModel
    {
        public GameResultViewerModel(GameResult gameResult, GameBoardConfiguration gameBoardConfiguration, bool createReplayer)
        {
            if (createReplayer)
            {
                switch (gameBoardConfiguration)
                {
                    case TicTacToeGameBoardConfiguration ticTacToeGameBoardConfiguration:
                        this.GameReplayer = new TicTacToeReplayer(ticTacToeGameBoardConfiguration, gameResult.Moves);
                        break;
                }
            }

            this.GameResult = gameResult;
        }

        public IGameReplayer? GameReplayer { get; }

        public GameResult GameResult { get; }
    }


    public interface IGameReplayer
    {
        public string GetHtmlBoardState(int moveIndex);

        public int GetMoveCount();
    }

    public class TicTacToeReplayer : IGameReplayer
    {
        private TicTacToeGameBoardConfiguration _boardConfiguration;

        private List<string> _boardStates = new List<string>();

        private List<Move> _moves;
        public TicTacToeReplayer(TicTacToeGameBoardConfiguration ticTacToeGameBoardConfiguration, List<Move> moves)
        {
            this._boardConfiguration = ticTacToeGameBoardConfiguration;
            this._moves = moves;

            this.CreateBoardStates();
        }

        private void CreateBoardStates()
        {
            int size = this._boardConfiguration.BoardSize;
            int[,] board = new int[size, size];
            int idx = 0;
            this._boardStates.Add(MakeHtmlBoard(board));

            foreach (Move move in this._moves)
            {
                board[(int)move.Commands[0], (int)move.Commands[1]] = (idx % 2) + 1;
                idx++;

                this._boardStates.Add(MakeHtmlBoard(board));
            }

        }

        private string MakeHtmlBoard(int[,] board)
        {
            StringBuilder stringBuilder= new StringBuilder();
            stringBuilder.AppendLine("<div>");
            stringBuilder.AppendLine("<table>");
            stringBuilder.AppendLine("<tbody>");

            for (int i = 0; i < board.GetLength(0); i++)
            {
                stringBuilder.AppendLine("<tr>");
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    stringBuilder.AppendLine($"<td>{board[i, j]}</td>");
                }

                stringBuilder.AppendLine("</tr>");
            }


            stringBuilder.AppendLine("</tbody>");
            stringBuilder.AppendLine("</table>");
            stringBuilder.AppendLine("</div>");

            return stringBuilder.ToString();
        }

        public string GetHtmlBoardState(int moveIndex)
            => this._boardStates[moveIndex];

        public int GetMoveCount()
            => this._moves.Count + 1;
    }
}
