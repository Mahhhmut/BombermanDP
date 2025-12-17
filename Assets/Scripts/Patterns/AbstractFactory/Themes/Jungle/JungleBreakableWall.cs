using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "JungleBreakableWall", menuName = "Bomberman/Product/Jungle Breakable")]
public class JungleBreakableWall : ScriptableObject, IBreakableWall
{
    [SerializeField] private TileBase _JungleBreakTile;

    public TileBase UnityTile => _JungleBreakTile;

    public void Explode()
    {
        Debug.Log("jungle wall is destroyed.");
    }
}
