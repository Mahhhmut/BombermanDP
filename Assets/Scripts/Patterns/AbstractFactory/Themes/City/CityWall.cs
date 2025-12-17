using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "CityWall", menuName = "Bomberman/Product/City Unbreakable")]
public class CityWall : ScriptableObject, IWall
{
    [SerializeField] private TileBase _CityWallTile;

    public TileBase UnityTile => _CityWallTile;
}
