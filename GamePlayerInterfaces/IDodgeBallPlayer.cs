using GamePlayerInterfaces.DodgeBall;

namespace GamePlayerInterfaces
{
    public interface IDodgeBallPlayer
    {
        public void Initialize(bool isStarting);

        public DodgeBallPlayerMove NextMove(IDodgeBallGameState state);
    }
}
