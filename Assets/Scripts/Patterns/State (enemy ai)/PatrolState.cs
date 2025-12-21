using UnityEngine;
public class PatrolState : IEnemyState
{
    private Vector2 _moveDir;

    public PatrolState() => _moveDir = GetRandomDirection();

    public void UpdateState(EnemyAI enemy)
    {
        float dist = enemy.speed * Time.fixedDeltaTime;

        // Önü müsaitse yürü
        if (enemy.CanMove(_moveDir, dist))
        {
            enemy.MovePhysical(_moveDir);
        }
        else
        {
            // Duvara çarptığı an (Player gibi kalır), yeni yön seçer
            _moveDir = GetRandomDirection();
        }
    }

    private Vector2 GetRandomDirection()
    {
        Vector2[] dirs = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        return dirs[Random.Range(0, dirs.Length)];
    }
}