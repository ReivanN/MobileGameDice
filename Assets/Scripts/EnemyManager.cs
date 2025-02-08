using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Настройки")]
    public float detectionRadius = 100f; 
    public int requiredFighters = 10;

    [Header("Победа")]
    public GameObject victoryUI; 
    public TextMeshProUGUI score;

    void Update()
    {
        
        int fighterCount = CountFightersOnScene();
        score.text = fighterCount.ToString();
        Debug.Log($"Текущее количество бойцов: {fighterCount}");

        if (fighterCount >= requiredFighters)
        {
            TriggerVictory();
        }
    }

    int CountFightersOnScene()
    {
        // Ищем всех бойцов в радиусе
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        int fighterCount = 0;

        foreach (var collider in colliders)
        {
            IFraction fraction = collider.GetComponent<IFraction>();

            if (fraction != null && fraction.GetTeam() == Fraction.Team.Enemy)
            {
                fighterCount++;
            }
        }

        return fighterCount;
    }

    void TriggerVictory()
    {
        Debug.Log("Победа! Достигнуто нужное количество бойцов!");

        if (victoryUI != null)
        {
            victoryUI.SetActive(true); // Отображаем экран победы
        }

        // Остановка игры или другие действия
        //Time.timeScale = 0f; // Останавливаем игру
    }
}
