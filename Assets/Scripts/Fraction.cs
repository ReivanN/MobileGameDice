using UnityEngine;

public class Fraction : MonoBehaviour, IFraction
{
    public int health = 50;
    public enum Team
    {
        Ally,
        Enemy,
    }

    public enum Priority
    {
        High,
        Low,
    }

    [SerializeField] private Team team;

    public Team GetTeam()
    {
        return team;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
