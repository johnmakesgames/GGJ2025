using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField]
    int Health;

    [SerializeField]
    int Damage;

    [SerializeField]
    float knockdownForce;

    [SerializeField]
    float stunDuration;

    public void DealDamage(int dmg)
    {
        Health -= dmg;
    }

    public bool IsAlive()
    {
        return Health > 0;
    }

    public int GetDamageFromEnemy()
    {
        return Damage;
    }

    public float GetKnockbackForce()
    {
        return knockdownForce;
    }

    public float GetStunDuration()
    {
        return stunDuration;
    }
}
