using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundUI : MonoBehaviour
{
    public TextMeshProUGUI phaseText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI TimeDuration;
    public float roundTimer;
    void OnEnable()
    {
        RoundManager.OnPhaseStart += UpdateUI;
    }

    void OnDisable()
    {
        RoundManager.OnPhaseStart -= UpdateUI;
    }

    private void UpdateUI(RoundManager.RoundPhase phase, int roundNumber)
    {
        roundText.text = "�����: " + roundNumber;
        switch (phase)
        {
            case RoundManager.RoundPhase.Preparation:
                phaseText.text = "����: ����������";
                roundTimer = FindAnyObjectByType<RoundManager>().preparationDuration;
                TimeDuration.text ="����� ����������" + roundTimer.ToString();
                StartCoroutine(UpdateTimer());
                break;
            case RoundManager.RoundPhase.Battle:
                phaseText.text = "����: �����";
                roundTimer = FindAnyObjectByType<RoundManager>().battleDuration;
                TimeDuration.text = "����� �����" + roundTimer.ToString();
                StartCoroutine(UpdateTimer());
                break;
            case RoundManager.RoundPhase.Scoring:
                phaseText.text = "����: ������� �����";
                roundTimer = FindAnyObjectByType<RoundManager>().scoringDuration;
                TimeDuration.text = "����� �������� �����" + roundTimer.ToString();
                StartCoroutine(UpdateTimer());
                break;
        }
    }

    private IEnumerator UpdateTimer()
    {
        while (roundTimer > 0)
        {
            roundTimer -= Time.deltaTime;
            TimeDuration.text = "�����: " + Mathf.CeilToInt(roundTimer);
            yield return null;
        }

    }
}
