using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "CityGround", menuName = "Bomberman/Product/City Walkable")]
public class CityGround : ScriptableObject, IGround
{
    [SerializeField] private TileBase _CityGroundTile;

    public TileBase UnityTile => _CityGroundTile;
}
