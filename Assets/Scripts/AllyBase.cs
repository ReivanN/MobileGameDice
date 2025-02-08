using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AllyBase : MonoBehaviour, IFraction
{
    [Header("Основные")]
    public int health = 50;
    public float detectionRadius = 25f;
    public Fraction.Team team = Fraction.Team.Ally;
    public GameObject winPanel;
    public TextMeshProUGUI TextMeshProUGUI;

    public Fraction.Team GetTeam()
    {
        return team;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
            winPanel.SetActive(true);
            Time.timeScale = 0; 
        }
    }
    public void Update()
    {
        TextMeshProUGUI.text = health.ToString();
    }

}
