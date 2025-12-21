using UnityEngine;
using UnityEngine.Tilemaps;

public class DesertStrategy : IMapThemeStrategy
{
    public IMapFactory GetFactory() => new DesertMapFactory();
}
