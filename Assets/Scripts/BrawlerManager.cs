using TMPro;
using UnityEngine;

public class BrawlerManager : MonoBehaviour
{
    [Header("���������")]
    public float detectionRadius = 100f; 
    public int requiredFighters = 10;

    [Header("������")]
    public GameObject victoryUI; 
    public TextMeshProUGUI score;

    void Update()
    {
        
        int fighterCount = CountFightersOnScene();
        score.text = fighterCount.ToString();
        Debug.Log($"������� ���������� ������: {fighterCount}");

        if (fighterCount >= requiredFighters)
        {
            TriggerVictory();
        }
    }

    int CountFightersOnScene()
    {
        // ���� ���� ������ � �������
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        int fighterCount = 0;

        foreach (var collider in colliders)
        {
            IFraction fraction = collider.GetComponent<IFraction>();

            if (fraction != null && fraction.GetTeam() == Fraction.Team.Ally)
            {
                fighterCount++;
            }
        }

        return fighterCount;
    }

    void TriggerVictory()
    {
        Debug.Log("������! ���������� ������ ���������� ������!");

        if (victoryUI != null)
        {
            victoryUI.SetActive(true);
        }

        
        //Time.timeScale = 0f;
    }
}
