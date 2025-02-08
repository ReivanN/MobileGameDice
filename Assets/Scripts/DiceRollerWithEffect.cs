using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DiceRollerWithEffect : MonoBehaviour
{
    public TextMeshProUGUI resultTextPlayer;
    public TextMeshProUGUI resultTextBot;
    public GameObject playerTurn;
    public GameObject enemyTurn;
    public Button playerDiceButton;
    public Button botDiceButton;
    public float rollDuration = 0.5f;
    public Color finalColor = Color.green;
    private LoaderPrefabs loaderPrefabs;
    private bool isPlayerTurn = true;
    public AudioSource playerDiceAudioSource;
    public AudioClip playerDiceAudioClip;

    [Header("Ходы")]
    private int playerRollsLeft = 10;
    private int botRollsLeft = 10;

    public TextMeshProUGUI playersRolls;
    public TextMeshProUGUI botRolls;

    private void Start()
    {
        loaderPrefabs = FindAnyObjectByType<LoaderPrefabs>();

        // Устанавливаем обработчик для кнопки игрока
        playerDiceButton.onClick.AddListener(() => StartCoroutine(PlayerTurn()));

        // Деактивируем кнопку бота в начале игры
        botDiceButton.interactable = false;
    }

    private void Update()
    {
        playersRolls.text = playerRollsLeft.ToString();
        botRolls.text = botRollsLeft.ToString();
    }

    private IEnumerator PlayerTurn()
    {
        if (!isPlayerTurn || playerRollsLeft <= 0) yield break;

        playerDiceButton.interactable = false; // Отключаем кнопку игрока
        playerRollsLeft--;

        yield return StartCoroutine(RollEffect(resultTextPlayer));

        isPlayerTurn = false;
        botDiceButton.interactable = botRollsLeft > 0;
        enemyTurn.SetActive(true);
        playerTurn.SetActive(false);

        // Запускаем ход бота автоматически
        if (botRollsLeft > 0)
        {
            StartCoroutine(BotTurn());
        }
    }

    private IEnumerator BotTurn()
    {
        if (isPlayerTurn || botRollsLeft <= 0) yield break;

        botRollsLeft--;

        // Задержка перед броском бота
        yield return new WaitForSeconds(0f);

        yield return StartCoroutine(RollEffect(resultTextBot));

        isPlayerTurn = true;
        playerDiceButton.interactable = playerRollsLeft > 0;
        playerTurn.SetActive(true);
        enemyTurn.SetActive(false);
    }

    private IEnumerator RollEffect(TextMeshProUGUI resultText)
    {
        float elapsed = 0f;

        resultText.color = Color.black;
        resultText.fontStyle = FontStyles.Normal;
        playerDiceAudioSource.PlayOneShot(playerDiceAudioClip);

        while (elapsed < rollDuration)
        {
            int tempResult = Random.Range(1, 7);
            resultText.text = tempResult.ToString();

            elapsed += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        int finalResult = Random.Range(1, 7);
        resultText.text = finalResult.ToString();

        resultText.color = finalColor;
        resultText.fontStyle = FontStyles.Bold;

        Debug.Log("Выпало число: " + finalResult);

        // Выполнение действий в зависимости от результата броска
        if (resultText == resultTextPlayer && isPlayerTurn)
        {
            if (finalResult == 1 || finalResult == 2)
                loaderPrefabs.Start1();
            else if(finalResult == 3 || finalResult == 4)
                loaderPrefabs.Start1_1();
            else if(finalResult == 5 || finalResult == 6)
                loaderPrefabs.Start3();
        }
        else if (resultText == resultTextBot && !isPlayerTurn)
        {
            if (finalResult == 1 || finalResult == 2)
                loaderPrefabs.Start2();
            else if (finalResult == 3 || finalResult == 4)
                loaderPrefabs.Start2_2();
            else if (finalResult == 5 || finalResult == 6)
                loaderPrefabs.Start3_3();
        }

        yield return new WaitForSeconds(0f);

        // Проверяем, есть ли ещё броски у игроков
        if (playerRollsLeft <= 0 && botRollsLeft <= 0)
        {
            playerDiceButton.interactable = false;
            botDiceButton.interactable = false;
            Debug.Log("Игра окончена: попытки бросков исчерпаны.");
        }
    }
}
