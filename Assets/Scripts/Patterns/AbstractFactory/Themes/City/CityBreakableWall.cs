using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "CityBreakableWall", menuName = "Bomberman/Product/City Breakable")]
public class CityBreakableWall : ScriptableObject, IBreakableWall
{
    [SerializeField] private TileBase _CityBreakTile;

    public TileBase UnityTile => _CityBreakTile;

    public void Explode()
    {
        Debug.Log("City Wall Exploded.");
    }
}
