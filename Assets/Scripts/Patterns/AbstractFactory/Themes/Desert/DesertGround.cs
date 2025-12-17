using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "DesertGround", menuName = "Bomberman/Product/Desert Walkable")]
public class DesertGround : ScriptableObject, IGround
{
    [SerializeField] private TileBase _DesertGroundTile;
    public TileBase UnityTile => _DesertGroundTile;
}
