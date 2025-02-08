using UnityEngine;
using static Fraction;

public class Bullet : MonoBehaviour
{
    public Team team;
    public int damage = 3;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IFraction fraction))
        {
            Debug.LogError("Log 1");
            if (fraction != null && team == Team.Ally && fraction.GetTeam() == Fraction.Team.Enemy) 
            {
                fraction.TakeDamage(damage);
                Debug.LogError("Log 2");
            }

            if(fraction != null && team == Team.Enemy && fraction.GetTeam() == Fraction.Team.Ally) 
            {
                fraction.TakeDamage(damage);
            }
                
        }
        Destroy(gameObject);
    }
}
