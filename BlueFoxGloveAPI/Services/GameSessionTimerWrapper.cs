using BlueFoxGloveAPI.Services.Interfaces;
using System.Threading;

namespace BlueFoxGloveAPI.Services
{
    public class GameSessionTimerWrapper: IGameSessionTimerWrapper
    {
        private Timer _timer;
        private const double _gameSessionDurationInSeconds = 60;
        public event Action Tick;

        public void Start()
        {
            _timer = new Timer(_ => Tick?.Invoke(), null, TimeSpan.FromMinutes(_gameSessionDurationInSeconds), Timeout.InfiniteTimeSpan);
        }

        public void Stop()
        {
            Console.WriteLine("Lobby Timer Stopping...");
            _timer.Dispose();
        }
    }
}