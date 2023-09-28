using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MimicMovement : AbstractEnemyMovement
{
    private bool isInsideRange;

    [SerializeField]
    private GameObject chestUI;

    public GameObject chestUIInstance;
    private TMP_Text chestText;

    private void Awake()
    {
        Initialize();
        chestUIInstance = Instantiate(chestUI);
        chestText = chestUIInstance.GetComponentInChildren<TMP_Text>();
        chestUIInstance.SetActive(false);
    }

    void Start()
    {
        StartCoroutine(PlayAnimationEvery10Seconds());
    }

    private IEnumerator BattleStart()
    {
        animator.SetFloat("doAnimation", 1);
        chestUIInstance.SetActive(true);
        chestText.SetText("¡Es un mímico!");
        yield return new WaitForSeconds(0.5f);
        chestUIInstance.SetActive(false);
        Time.timeScale = 1f;
        StartCoroutine(BattleManager.Instance.StartBattle(gameObject));
    }

    protected override void FixedUpdate()
    {
        bool isSpacePressed = Input.GetKeyDown(KeyCode.Space);
        if (isSpacePressed && isInsideRange)
        {
            StartCoroutine(BattleStart());
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            isInsideRange = true;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            isInsideRange = false;
        }
    }



    private IEnumerator PlayAnimationEvery10Seconds()
    {
        while (true)
        {
            animator.SetFloat("doAnimation", 1);
            yield return new WaitForSeconds(0.75f);
            animator.SetFloat("doAnimation", 0);
            yield return new WaitForSeconds(10f);
        }
    }
}
