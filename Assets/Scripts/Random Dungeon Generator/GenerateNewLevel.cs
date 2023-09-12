using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateNewLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Time.timeScale = 0f; // Pausar el juego
            StartCoroutine(DifficultyController.Instance.DoTransition());
        }
    }

}
