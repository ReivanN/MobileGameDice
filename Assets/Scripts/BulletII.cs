using UnityEngine;
using static Fraction;

public class BulletII : MonoBehaviour
{
    public Team team;
    public int damage = 3;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out IFraction fraction))
        {
            Debug.LogError("Log 1");
            if (fraction != null && fraction.GetTeam() == Fraction.Team.Ally) 
            {
                fraction.TakeDamage(damage);
                Debug.LogError("Log 2");
            }
                
        }
        Destroy(gameObject);
    }
}
