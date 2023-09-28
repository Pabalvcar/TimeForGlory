using System.Collections;
using System.Collections.Generic;
using TMPro;
using Pathfinding;
using UnityEngine;
using System;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, FINISH }
public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    [SerializeField]
    private GameObject battleUI;
    [SerializeField]
    private GameObject battleTransitionScreen;
    [SerializeField]
    private TMP_Text battleText;

    [SerializeField]
    private TMP_Text hpText;
    [SerializeField]
    private Slider hpSlider;

    [SerializeField]
    private TMP_Text mpText;
    [SerializeField]
    private Slider mpSlider;

    [SerializeField]
    private TMP_Text enemyHpText;
    [SerializeField]
    private Slider enemyHpSlider;
    [SerializeField]
    private GameObject enemyImg;

    public Button attackButton;
    public Button magicButton;

    [SerializeField]
    private BattleState battleState;

    public GameObject player;
    public PlayerBattle playerStats;
    public int originalDefense;

    public GameObject enemy;
    public AbstractEnemyBattle enemyStats;
    public string enemyType;

    public GameObject[] enemies;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip normalAttack;
    [SerializeField]
    private AudioClip magicAttack;
    [SerializeField]
    private AudioClip victoryTheme;
    [SerializeField]
    private AudioClip defeatTheme;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public IEnumerator StartBattle(GameObject enemyToFight)
    {

        battleState = BattleState.START;
        attackButton.interactable = false;
        magicButton.interactable = false;

        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerBattle>();
        originalDefense = playerStats.defense;

        enemy = enemyToFight;

        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        player.GetComponent<PlayerMovement>().enabled = false;

        int closeEnemies = 0;

        foreach (var e in enemies)
        {
            e.GetComponent<AbstractEnemyMovement>().enabled = false;
            e.GetComponent<Collider2D>().enabled = false;
            if (e != enemyToFight)
            {
                float distanceToEnemyToFight = Vector3.Distance(e.transform.position, enemyToFight.transform.position);
                if (distanceToEnemyToFight < 6f)
                {
                    DespawnEnemy(e);
                    closeEnemies += 1;
                }
            }
        }

        enemyStats = enemy.GetComponent<AbstractEnemyBattle>();
        enemyStats.UpdateStats(closeEnemies);
        enemyStats.updateDropGold(closeEnemies);

        enemyType = enemy.name.Replace("(Clone)", "");
        battleTransitionScreen.SetActive(true);
        battleUI.SetActive(true);
        battleText.SetText("¡" + enemyType + " de nivel " + (enemyStats.level + 1) + " bloquea tu camino!");

        SetPlayerHP();

        SetPlayerMP();

        SetEnemyHP();

        enemyImg.GetComponent<Image>().sprite = enemyStats.baseStats.battleSprite;

        CanvasGroup canvasGroup = battleTransitionScreen.GetComponent<CanvasGroup>();
        float fadeDuration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        PlayerTurn();
    }

    void PlayerTurn()
    {
        battleState = BattleState.PLAYERTURN;
        battleText.SetText("Elige tu siguiente movimiento");
        attackButton.interactable = true;
        if (playerStats.canUseMagic)
            magicButton.interactable = true;
        else magicButton.interactable = false;
    }

    IEnumerator EnemyTurn()
    {
        battleState = BattleState.ENEMYTURN;
        attackButton.interactable = false;
        magicButton.interactable = false;

        float specialChance = GameManager.Instance.specialAttackChance;
        float randomValue = Random.value;
        if (randomValue < specialChance)
        {
            battleText.SetText("¡" + enemyType + " va a usar su ataque especial!");
            yield return new WaitForSeconds(2f);
            string description = enemyStats.SpecialAttack(enemyStats.strenght);
            battleText.SetText(description);
            SetPlayerHP();
            SetPlayerMP();
            SetEnemyHP();
        } else
        {
            int damageTaken = playerStats.TakeDamage(enemyStats.strenght);
            SetPlayerHP();
            battleText.SetText(enemyType + " te ataca y recibes " + damageTaken + " puntos de daño");
        }

        yield return new WaitForSeconds(2f);

        if (playerStats.currentHP <= 0)
        {
            StartCoroutine(EndGame());
        } else
        {
            PlayerTurn();
        }


    }

    IEnumerator PlayerAttack()
    {
        attackButton.interactable = false;
        magicButton.interactable = false;
        audioSource.clip = normalAttack;
        audioSource.Play();

        string description = enemyStats.TakeDamage(playerStats.strenght);

        if (enemyStats.isPoisoned)
        {
            description = description + "\n" + enemyStats.TakePoisonDamage();
        }

        SetPlayerHP();
        SetEnemyHP();

        battleText.SetText(description);

        yield return new WaitForSeconds(2f);

        if (enemyStats.currentHP == 0)
        {
            StartCoroutine(EndBattle());
        } else
        {
            StartCoroutine(EnemyTurn());
        }

    }

    IEnumerator PlayerMagicAttack()
    {
        attackButton.interactable = false;
        magicButton.interactable = false;
        audioSource.clip = magicAttack;
        audioSource.Play();

        string description = "No tienes suficiente MP para usar tu ataque mágico \n(coste actual: " + playerStats.magicAttackCostMP + " MP)";

        if (playerStats.currentMP >= playerStats.magicAttackCostMP)
        {
            playerStats.LoseMP(playerStats.magicAttackCostMP);
            description = enemyStats.TakeMagicDamage(playerStats.intelligence);

            if (enemyStats.isPoisoned)
            {
                description = description + "\n" + enemyStats.TakePoisonDamage();
            }

            SetPlayerHP();
            SetPlayerMP();
            SetEnemyHP();

            battleText.SetText(description);

            yield return new WaitForSeconds(2f);

            if (enemyStats.currentHP == 0)
            {
                StartCoroutine(EndBattle());
            }
            else
            {
                StartCoroutine(EnemyTurn());
            }

        } else
        {
            battleText.SetText(description);
            yield return new WaitForSeconds(2f);
            PlayerTurn();
        }

    }

    IEnumerator EndBattle()
    {
        battleState = BattleState.FINISH;
        audioSource.clip = victoryTheme;
        audioSource.Play();

        int goldAmount = enemyStats.goldDrop;

        playerStats.GetGold(goldAmount);

        battleText.SetText("¡Has derrotado a " + enemyType + "!\n" + "Has conseguido " + goldAmount + " monedas de oro");

        yield return new WaitForSeconds(3f);

        playerStats.defense = originalDefense;

        battleTransitionScreen.SetActive(false);
        battleUI.SetActive(false);

        player.GetComponent<PlayerMovement>().enabled = true;

        playerStats.updateUI();

        DespawnEnemy(enemy);

        foreach (var e in enemies)
        {
            if (e != null)
            {
                e.GetComponent<AbstractEnemyMovement>().enabled = true;
                e.GetComponent<Collider2D>().enabled = true;
            }
        }

    }

    IEnumerator EndGame()
    {
        battleState = BattleState.FINISH;
        battleText.SetText("¡Te han derrotado en combate!\nFIN DE LA PARTIDA");
        audioSource.clip = defeatTheme;
        audioSource.Play();

        PlayerPrefs.SetInt("Score", GameManager.Instance.currentLevel);

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("LeaderboardsUI");
    }

    private void DespawnEnemy(GameObject enemy)
    {
        if (enemy.GetComponent<MimicMovement>())
        {
            MimicMovement mimicScript = enemy.GetComponent<MimicMovement>();
            Destroy(mimicScript.chestUIInstance);
        }
        Destroy(enemy);
    }

    public void OnClickAttackButton()
    {
        if (battleState != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void OnClickMagicAttackButton()
    {
        if (battleState != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerMagicAttack());
    }

    private void SetPlayerMP()
    {
        if (playerStats.currentMP < 0)
            playerStats.currentMP = 0;

        mpText.SetText("MP " + playerStats.currentMP + "/" + playerStats.maxMP);
        mpSlider.maxValue = playerStats.maxMP;
        mpSlider.value = playerStats.currentMP;
    }

    private void SetPlayerHP()
    {
        if (playerStats.currentHP < 0)
            playerStats.currentHP = 0;

        hpText.SetText("HP " + playerStats.currentHP + "/" + playerStats.maxHP);
        hpSlider.maxValue = playerStats.maxHP;
        hpSlider.value = playerStats.currentHP;
    }

    private void SetEnemyHP()
    {
        if (enemyStats.currentHP < 0)
            enemyStats.currentHP = 0;

        enemyHpText.SetText("HP " + enemyStats.currentHP + "/" + enemyStats.maxHP);
        enemyHpSlider.maxValue = enemyStats.maxHP;
        enemyHpSlider.value = enemyStats.currentHP;
    }

}
