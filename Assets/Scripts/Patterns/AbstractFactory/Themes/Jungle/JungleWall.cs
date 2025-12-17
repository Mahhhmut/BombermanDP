using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "JungleWall", menuName = "Bomberman/Product/Jungle Unbreakable")]
public class JungleWall : ScriptableObject, IWall
{
    [SerializeField] private TileBase _JungleWallTile;

    public TileBase UnityTile => _JungleWallTile;

}
