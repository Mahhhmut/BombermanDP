using UnityEngine;

public class BombCountDecorator : PlayerAbilityDecorator
{
    public BombCountDecorator(IPlayerAbility decoratedAbility) : base(decoratedAbility)
    {
        // Temel sınıfın yapıcı metodunu çağırır.
    }

    //Yalnızca değişecek özellik burada override edilir.
    public override int BombCount => _decoratedAbility.BombCount +1; // 1 bomba ekler

     // Diğer tüm metotlar (Move, PlaceBomb, MovementSpeed, BombRange)
    // soyut PlayerAbilityDecorator'dan miras aldığı için otomatik olarak
    // _wrappedAbility'ye (bir sonraki halkaya) yönlendirilir.
}
