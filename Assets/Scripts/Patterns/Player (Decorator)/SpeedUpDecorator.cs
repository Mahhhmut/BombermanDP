using UnityEngine;

public class SpeedUpDecorator : PlayerAbilityDecorator
{
    private const float SPEED_INCREASE = 1.5f;

    public SpeedUpDecorator(IPlayerAbility decoratedAbility) : base(decoratedAbility)
    {
        
    }
    public override float MovementSpeed => _decoratedAbility.MovementSpeed + SPEED_INCREASE;

    // Diğer tüm yetenekler (Bomba Sayısı, Menzil, vb.) otomatik olarak zincirin bir önceki halkasına iletilir.
}
