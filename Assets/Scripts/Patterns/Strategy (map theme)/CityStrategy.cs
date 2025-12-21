using UnityEngine;
using UnityEngine.Tilemaps;


public class CityStrategy : IMapThemeStrategy
{
    public IMapFactory GetFactory() => new CityMapFactory();
}
