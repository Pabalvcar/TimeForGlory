using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Pathfinding;

public class DifficultyController : MonoBehaviour
{
    public static DifficultyController Instance { get; private set; }

    // ################ AJUSTES DE DIFICULTAD ################

    public int currentlevel = 0;

    public int enemyAmountPerRoom = 1;

    public float slimeSpawnWeight = 0.4f;

    public float skeletonSpawnWeight = 0.39f;

    public float bansheeSpawnWeight = 0.15f;

    public float demonSpawnWeight = 0.05f;

    public float mimicSpawnWeight = 0.01f;

    // #######################################################

    [SerializeField]
    private TMP_Text cornerLevelText;

    [SerializeField]
    private TMP_Text levelTransitionText;

    [SerializeField]
    private GameObject levelTransitionScreen;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator DoTransition()
    {
        cornerLevelText.SetText("Planta " + currentlevel);
        levelTransitionText.SetText("Planta " + currentlevel);
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

        yield return new WaitForSecondsRealtime(0.5f);
        levelTransitionScreen.SetActive(false);


    }
}
