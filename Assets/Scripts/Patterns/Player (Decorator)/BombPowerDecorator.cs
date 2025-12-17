using UnityEngine;

public class BombPowerDecorator : PlayerAbilityDecorator
{
    private const int RANGE_INCREASE = 1;

    public BombPowerDecorator(IPlayerAbility decoratedAbility) : base(decoratedAbility)
    {
        //Temel sınıfın yapıcı metodu burada çağırılır.
    }

    public override int BombRange => _decoratedAbility.BombRange + RANGE_INCREASE;

    // Diğer tüm yetenekler (Hız, Bomba Sayısı, PlaceBomb, Move) 
    // PlayerAbilityDecorator üzerinden otomatik olarak zincirin bir önceki halkasına yönlendirilir.
}
