namespace BlueFoxGloveAPI.Services.Interfaces
{
    public interface IGameSessionTimerWrapper
    {
        void Start();
        void Stop();
        event Action Tick;
    }
}