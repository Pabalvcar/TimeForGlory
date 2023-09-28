using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Pathfinding;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // ################ AJUSTES DE DIFICULTAD ################

    public int currentLevel = 0;

    public int enemyLevel = 0;

    public int enemyAmountPerRoom = 1;

    public int extraEnemies = 0;

    public int chestNumber = 3;

    public int chestGoldCost = 30;

    private float startTime;

    private float currentTime;

    private int minutes;

    private int seconds;

    private int currentMinute = 0;

    public float slimeSpawnWeight = 0.4f;

    public float skeletonSpawnWeight = 0.39f;

    public float bansheeSpawnWeight = 0.15f;

    public float demonSpawnWeight = 0.05f;

    public float mimicSpawnWeight = 0.01f;

    public float specialAttackChance = 0.20f;

    // #######################################################

    [SerializeField]
    private TMP_Text cornerLevelText;

    [SerializeField]
    private TMP_Text levelTransitionText;

    [SerializeField]
    private GameObject levelTransitionScreen;

    [SerializeField]
    private TMP_Text timeText;

    public TMP_Text hpText;

    public TMP_Text mpText;

    public TMP_Text strenghtText;

    public TMP_Text intelligenceText;

    public TMP_Text defenseText;

    public TMP_Text goldText;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        levelTransitionScreen.SetActive(true);
        levelTransitionText.alpha = 1f;
        StartCoroutine(Instance.DoTransition());
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = Time.time - startTime;
        minutes = Mathf.FloorToInt(currentTime / 60);
        seconds = Mathf.FloorToInt(currentTime % 60);
        timeText.SetText(string.Format("{0:00}:{1:00}", minutes, seconds));

        if (minutes > currentMinute)
        {
            currentMinute += 1;
            enemyLevel += 1;
        }
    }

    public IEnumerator DoTransition()
    {
        currentLevel += 1;
        enemyLevel += 1;
        if ((currentLevel % 5) == 0)
        {
            extraEnemies += 2;
        }

        cornerLevelText.SetText("Planta " + currentLevel);
        levelTransitionText.SetText("Planta " + currentLevel);

        levelTransitionScreen.SetActive(true);
        CanvasGroup canvasGroup = levelTransitionScreen.GetComponent<CanvasGroup>();
        float fadeDuration = 0.5f;
        float textFadeDuration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            canvasGroup.alpha = alpha;
            levelTransitionText.alpha = alpha;
            yield return null;
        }

        CorridorBasedDungeonGenerator.Instance.GenerateDungeon();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerBattle playerStats = player.GetComponent<PlayerBattle>();

        playerStats.RecoverHP(Mathf.RoundToInt(playerStats.maxHP * 0.5f));
        playerStats.RecoverMP(Mathf.RoundToInt(playerStats.maxMP * 0.5f));

        playerStats.updateUI();

        elapsedTime = 0f;

        Time.timeScale = 1f; // Volver a reanudar el juego
        yield return new WaitForSecondsRealtime(0.5f);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        elapsedTime = 0f;

        Time.timeScale = 0f;

        AstarPath path = AstarPath.active;
        path.Scan();

        Time.timeScale = 1f;

        while (elapsedTime < textFadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsedTime / fadeDuration); // Calculate the new alpha value
            levelTransitionText.alpha = alpha;
            yield return null;
        }

        levelTransitionScreen.SetActive(false);

    }

}
