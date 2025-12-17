using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "DesertWall", menuName = "Bomberman/Product/Desert Unbreakable")]
public class DesertWall : ScriptableObject, IWall
{
    [SerializeField] private TileBase _DesertWallTile;

    public TileBase UnityTile => _DesertWallTile;
}
