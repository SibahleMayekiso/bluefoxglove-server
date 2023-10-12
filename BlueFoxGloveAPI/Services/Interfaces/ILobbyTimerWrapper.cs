namespace BlueFoxGloveAPI.Services.Interfaces
{
    public interface ILobbyTimerWrapper
    {
        void Start();
        void Stop();
        void Change(TimeSpan dueTime, TimeSpan period);
        event Action Tick;
    }
}