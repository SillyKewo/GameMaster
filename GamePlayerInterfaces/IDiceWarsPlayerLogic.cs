using Hexagonal;

namespace HexagonalTest.PlayerAPI
{
    public interface IDiceWarsPlayerLogic
    {
        void Initialize(Player player, IBoardState initialBoardState);

        void PlayTurn(IBoardState boardState);
    }
}