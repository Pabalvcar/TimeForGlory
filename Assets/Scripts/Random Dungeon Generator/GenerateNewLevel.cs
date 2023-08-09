using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateNewLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            DifficultyController.Instance.currentlevel += 1;
            if ((DifficultyController.Instance.currentlevel % 5) == 0)
            {
                DifficultyController.Instance.enemyAmountPerRoom += 1;
            }
            Time.timeScale = 0f; // Pausar el juego
            StartCoroutine(DifficultyController.Instance.DoTransition());
        }
    }

}
