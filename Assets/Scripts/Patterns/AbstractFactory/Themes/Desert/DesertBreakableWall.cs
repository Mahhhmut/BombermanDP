using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "DesertBreakableWall", menuName = "Bomberman/Product/Desert Breakable")]
public class DesertBreakableWall : ScriptableObject, IBreakableWall
{
    [SerializeField] private TileBase _DesertBreakTile;

    public TileBase UnityTile => _DesertBreakTile;

    public void Explode()
    {
        Debug.Log("Desert Wall is Exploded.");
    }
}
