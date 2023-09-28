using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Chest : MonoBehaviour
{

    private Animator animator;
    private bool isSpacePressed;
    private bool isInsideRange = false;
    private bool isOpen = false;
    private bool currentlyOpening = false;

    private PlayerBattle playerStats;

    [SerializeField]
    private GameObject chestUI;

    public GameObject chestUIInstance;
    private TMP_Text chestText;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip chestOpen;
    [SerializeField]
    private AudioClip chestBlocked;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        playerStats = player.GetComponent<PlayerBattle>();
        chestUIInstance = Instantiate(chestUI);
        chestText = chestUIInstance.GetComponentInChildren<TMP_Text>();
        chestUIInstance.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        isSpacePressed = Input.GetKeyDown(KeyCode.Space);

        if (!currentlyOpening && isSpacePressed && isInsideRange)
        {
            if (isOpen)
            {
                StartCoroutine(AlreadyOpenedDisplay());
            }
            else
            {
                if (GameManager.Instance.chestGoldCost <= playerStats.gold)
                {
                    StartCoroutine(ChestOpenedDisplay());
                }
                else
                {
                    StartCoroutine(NotEnoughGoldDisplay());
                }
            }
        }

    }

    private IEnumerator ChestOpenedDisplay()
    {
        currentlyOpening = true;

        chestUIInstance.SetActive(true);

        playerStats.LoseGold(GameManager.Instance.chestGoldCost);

        Time.timeScale = 0f;

        chestText.SetText("Abres el cofre");
        string rune = UpgradeRandomStat();
        playerStats.updateUI();
        yield return new WaitForSecondsRealtime(2f);

        audioSource.clip = chestOpen;
        audioSource.Play();

        chestText.SetText("¡Enhorabuena, has encontrado una runa " + rune);
        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = 1f;

        currentlyOpening = false;

        animator.SetBool("isOpen", true);
        isOpen = true;
        chestUIInstance.SetActive(false);
    }

    private IEnumerator NotEnoughGoldDisplay()
    {
        chestUIInstance.SetActive(true);
        chestText.SetText("No tienes suficiente oro" + "\nCoste: " + GameManager.Instance.chestGoldCost);
        audioSource.clip = chestBlocked;
        audioSource.Play();
        yield return new WaitForSeconds(2f);
        chestUIInstance.SetActive(false);
    }

    private IEnumerator AlreadyOpenedDisplay()
    {
        chestUIInstance.SetActive(true);
        chestText.SetText("Ya abriste este cofre");
        audioSource.clip = chestBlocked;
        audioSource.Play();
        yield return new WaitForSeconds(2f);
        chestUIInstance.SetActive(false);
    }

    public string UpgradeRandomStat()
    {
        float magicOrStat = Random.Range(0.01f, 1.00f);
        playerStats.numberOfUpgrades += 1;

        if (magicOrStat <= 0.1)
        {
            int randomMagic = Random.Range(0, 3);

            switch (randomMagic)
            {
                case 0:
                    playerStats.magicEffect = MagicEffect.LIFESTEAL;
                    playerStats.lifestealLevel += 1;
                    playerStats.maxHP += 50;
                    playerStats.currentHP += 50;
                    return "arcana: Vampirismo " + playerStats.lifestealLevel + "!\n Tu magia ahora convierte parte del daño inflingido en vida\n Además tu vida máxima aumenta en 50 puntos";
                case 1:
                    playerStats.magicEffect = MagicEffect.WEAKEN;
                    playerStats.weakenLevel += 1;
                    playerStats.strenght += 10;
                    return "arcana: Debilitamiento " + playerStats.weakenLevel + "!\n Tu magia ahora reduce la defensa de los enemigos\n Además tu fuerza aumenta en 10 puntos";
                case 2:
                    playerStats.magicEffect = MagicEffect.POISON;
                    playerStats.poisonLevel += 1;
                    playerStats.defense += 5;
                    return "arcana: Veneno " + playerStats.poisonLevel + "!\n Tu magia ahora envenena a los enemigos\n Además tu defensa aumenta en 5 puntos";
                case 3:
                    playerStats.magicEffect = MagicEffect.EXPLOSION;
                    playerStats.explosionLevel += 1;
                    playerStats.intelligence += 10;
                    playerStats.maxMP += 10;
                    playerStats.currentMP += 10;
                    return "arcana: Explosión mágica " + playerStats.explosionLevel + "!\n Tu magia ahora es muy potente, pero te hará daño a tí mismo\n Además tus MP y tu inteligencia aumentan en 10 puntos";
            }

        } else
        {
            int randomStat = Random.Range(0, 4);

            switch (randomStat)
            {
                case 0:
                    playerStats.maxHP += 25;
                    playerStats.currentHP += 25;
                    return "de vida!\n Tu vida máxima aumenta en 25 puntos";
                case 1:
                    playerStats.strenght += 5;
                    return "de fuerza!\n Tu fuerza aumenta en 5 puntos";
                case 2:
                    playerStats.intelligence += 5;
                    playerStats.maxMP += 10;
                    playerStats.currentMP += 10;
                    return "de magia!\n Tu inteligencia aumenta en 5 puntos y tus MP aumentan en 10 puntos";
                case 3:
                    playerStats.defense += 3;
                    return "de defensa!\n Tu defensa aumenta en 3 puntos";
            }
        }

        return "";
    }

        private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            isInsideRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            isInsideRange = false;
        }
    }

}
