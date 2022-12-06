using Entities;
using GameMaster.DataAccessLayer;

namespace PresentationLayer.GameMasterWebView
{
    public class TournamentCache
    {
        private List<TournamentResult> _tournamentCache = new List<TournamentResult>();

        private List<Player> _players = new List<Player>();

        private static TournamentCache? _singleton;

        private TournamentResultDataMapperXml _dataMapper;

        private static object _lock = new object();

        private CancellationTokenSource? _cancellationTokenSource;


        private TournamentCache(List<TournamentResult> tournamentResults, TournamentResultDataMapperXml dataMapper)
        {
            this._dataMapper= dataMapper;
            _tournamentCache= tournamentResults;
            this._players = tournamentResults.SelectMany(t => t.PlayerList).Distinct().ToList();
            TournamentCache._singleton = this;
        }

        public static TournamentCache Create(TournamentResultDataMapperXml dataMapper)
        {
            lock (_lock)
            {
                if (TournamentCache._singleton != null)
                {
                    return TournamentCache._singleton;
                }
                else
                {
                    return new TournamentCache(dataMapper.GetAllTournamentResults(), dataMapper);
                }
            }
        }

        public static TournamentCache? GetSingleton() => _singleton;

        public void StartCacheRefresh(TimeSpan interval)
        {
            if (this._cancellationTokenSource is not null)
            {
                return;
            }

            this._cancellationTokenSource = new CancellationTokenSource();
            var token = this._cancellationTokenSource.Token;
            Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    _tournamentCache = this._dataMapper.GetAllTournamentResults();
                    _players = _tournamentCache.SelectMany(t => t.PlayerList).Distinct().ToList();
                    await Task.Delay(interval, token);
                }
            }, token);
        }


        public void StopCacheRefresh()
        {
            if (this._cancellationTokenSource is null)
            {
                return;
            }

            this._cancellationTokenSource.Cancel();
            this._cancellationTokenSource = null;
        }


        public List<TournamentResult> TournamentResults => _tournamentCache;

        public List<Player> Players => _players;
    }
}
