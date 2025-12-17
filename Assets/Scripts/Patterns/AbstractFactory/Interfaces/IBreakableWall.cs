using UnityEngine;
using UnityEngine.Tilemaps;
public interface IBreakableWall
{
    TileBase UnityTile { get; }

    void Explode();
}
