using BlueFoxGloveAPI.Services.Interfaces;

namespace BlueFoxGloveAPI.Services
{
    public sealed class LobbyTimerWrapper: ILobbyTimerWrapper
    {
        private readonly Timer _timer;

        public event Action Tick;

        public LobbyTimerWrapper()
        {
            _timer = new Timer(_ => Tick?.Invoke(), null, Timeout.InfiniteTimeSpan, TimeSpan.FromSeconds(15));
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            Console.WriteLine("Lobby Timer Stopping...");
            _timer.Dispose();
        }

        public void Change(TimeSpan dueTime, TimeSpan period)
        {
            Console.WriteLine("Lobby Timer Restarting...");

            _timer.Change(dueTime, period);
        }
    }
}