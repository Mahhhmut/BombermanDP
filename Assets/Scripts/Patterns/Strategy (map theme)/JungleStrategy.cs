using UnityEngine;
using UnityEngine.Tilemaps;

public class JungleStrategy : IMapThemeStrategy
{
    public IMapFactory GetFactory() => new JungleMapFactory();
}
