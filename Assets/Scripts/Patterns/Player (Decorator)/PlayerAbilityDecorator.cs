
using UnityEngine; 

public abstract class PlayerAbilityDecorator : IPlayerAbility
{
    protected IPlayerAbility _decoratedAbility; 

    public PlayerAbilityDecorator(IPlayerAbility decoratedAbility)
    {
        _decoratedAbility = decoratedAbility;
    }

    // Özellikler (Properties)
    public virtual float MovementSpeed => _decoratedAbility.MovementSpeed;
    public virtual int BombCount => _decoratedAbility.BombCount;
    public virtual int BombRange => _decoratedAbility.BombRange;

    // Metotlar (Move ve PlaceBomb)
    // Varsayılan olarak sarmalanan nesnenin metodunu çağırır.
    // virtual olduğu için alt sınıflar (SpeedUpDecorator) bu davranışı değiştirebilir.

    // 💡 Düzeltilen Kısım: void metotların gövdesi olmalı ve sarmalanan nesneye yönlendirmeli
    public virtual void Move(Rigidbody2D rb, Vector2 direction)
    {
        _decoratedAbility.Move(rb, direction); 
    }

    public virtual void PlaceBomb()
    {
        _decoratedAbility.PlaceBomb();
    }
}