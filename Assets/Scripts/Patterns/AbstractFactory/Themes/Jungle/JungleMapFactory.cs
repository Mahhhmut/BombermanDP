using UnityEngine;
using System; // ArgumentException için

public class JungleMapFactory : IMapFactory
{
    // Yükleme sırasında Generic bir metot kullanmak kodu temizler (Opsiyonel)
    private T LoadAsset<T>(string assetName) where T : ScriptableObject
    {
        // ÖNEMLİ: Asset'in Assets/Resources/ klasöründe olduğundan emin olun!
        T asset = Resources.Load<T>(assetName);
        if (asset == null)
        {
            throw new Exception($"[JungleMapFactory] {assetName} isimli asset Resources klasöründe bulunamadı!");
        }
        return asset;
    }

    public IGround CreateGround()
    {
        // CityGround sınıfınızın ScriptableObject olduğunu belirtin.
        return LoadAsset<JungleGround>("JungleGroundAsset"); 
    }

    public IWall CreateWall()
    {
        // CityWall sınıfınızın ScriptableObject olduğunu varsayıyoruz.
        return LoadAsset<JungleWall>("JungleWallAsset"); 
    }

    public IBreakableWall CreateBreakableWall()
    {
        // CityBreakableWall sınıfınızın ScriptableObject olduğunu varsayıyoruz.
        return LoadAsset<JungleBreakableWall>("JungleBreakableWallAsset");
    }
}