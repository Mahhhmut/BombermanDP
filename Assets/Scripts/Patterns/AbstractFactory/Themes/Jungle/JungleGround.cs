using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "JungleGround", menuName = "Bomberman/Product/Jungle Walkable")]
public class JungleGround : ScriptableObject, IGround
{
    [SerializeField] private TileBase _JungleGroundTile;

    public TileBase UnityTile => _JungleGroundTile;
}
