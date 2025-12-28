public interface IGameObserver
{
    void OnGameEvent(string eventName, string message);
}