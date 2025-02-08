using UnityEngine;
using System.Collections;

public class RoundManager : MonoBehaviour
{
    public enum RoundPhase { Preparation, Battle, Scoring }
    public RoundPhase currentPhase = RoundPhase.Preparation;

    public int currentRound = 1;
    public float preparationDuration = 30f; 
    public float battleDuration = 60f;
    public float scoringDuration = 15f; 

    public delegate void PhaseEvent(RoundPhase phase, int roundNumber);
    public static event PhaseEvent OnPhaseStart;

    void Start()
    {
        StartCoroutine(RoundRoutine());
    }

    private IEnumerator RoundRoutine()
    {
        while (true)
        {
            currentPhase = RoundPhase.Preparation;
            OnPhaseStart?.Invoke(currentPhase, currentRound);
            yield return new WaitForSeconds(preparationDuration);

            currentPhase = RoundPhase.Battle;
            OnPhaseStart?.Invoke(currentPhase, currentRound);
            yield return new WaitForSeconds(battleDuration);

            currentPhase = RoundPhase.Scoring;
            OnPhaseStart?.Invoke(currentPhase, currentRound);
            yield return new WaitForSeconds(scoringDuration);

            currentRound++;
        }
    }
}
