using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance { get; private set; }
    private List<IGameObserver> _observers = new List<IGameObserver>();

    void Awake() => Instance = this;

    public void Register(IGameObserver observer) => _observers.Add(observer);
    public void Unregister(IGameObserver observer) => _observers.Remove(observer);

    public void Notify(string eventName, string message)
    {
        foreach (var observer in _observers)
        {
            observer.OnGameEvent(eventName, message);
        }
    }
}