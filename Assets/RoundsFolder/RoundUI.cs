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
        roundText.text = "Раунд: " + roundNumber;
        switch (phase)
        {
            case RoundManager.RoundPhase.Preparation:
                phaseText.text = "Фаза: Подготовка";
                roundTimer = FindAnyObjectByType<RoundManager>().preparationDuration;
                TimeDuration.text ="Время подготовки" + roundTimer.ToString();
                StartCoroutine(UpdateTimer());
                break;
            case RoundManager.RoundPhase.Battle:
                phaseText.text = "Фаза: Битва";
                roundTimer = FindAnyObjectByType<RoundManager>().battleDuration;
                TimeDuration.text = "Время битвы" + roundTimer.ToString();
                StartCoroutine(UpdateTimer());
                break;
            case RoundManager.RoundPhase.Scoring:
                phaseText.text = "Фаза: Подсчет очков";
                roundTimer = FindAnyObjectByType<RoundManager>().scoringDuration;
                TimeDuration.text = "Время подсчета очков" + roundTimer.ToString();
                StartCoroutine(UpdateTimer());
                break;
        }
    }

    private IEnumerator UpdateTimer()
    {
        while (roundTimer > 0)
        {
            roundTimer -= Time.deltaTime;
            TimeDuration.text = "Время: " + Mathf.CeilToInt(roundTimer);
            yield return null;
        }

    }
}
