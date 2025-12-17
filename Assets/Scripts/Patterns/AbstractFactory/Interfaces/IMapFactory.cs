using UnityEngine;

public interface IMapFactory
{
    IGround CreateGround();
    IWall CreateWall();
    IBreakableWall CreateBreakableWall();
}
