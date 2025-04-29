using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    public static int health = 3;
    private int previousHealth; // Per tenere traccia del cambio di health

    [SerializeField] GameObject live3;
    [SerializeField] GameObject live2;
    [SerializeField] GameObject live1;
    [SerializeField] GameObject live0;
    [SerializeField] GameObject pauseScene;

    private void Awake()
    {
        Debug.Log($"HealthSystem.Awake - health iniziale: {health}");
    }

    private void Start()
    {
        previousHealth = health; // Inizializza il valore precedente
        Debug.Log($"HealthSystem.Start - previousHealth inizializzato a: {previousHealth}");

        // Verifica subito se Ranking.Instance è disponibile
        if (Ranking.Instance != null)
        {
            Debug.Log("Ranking.Instance trovata correttamente all'avvio");
        }
        else
        {
            Debug.LogError("Ranking.Instance è NULL all'avvio!");
        }
    }

    private void Update()
    {
        if (health != previousHealth)
        {
            Debug.Log($"Health cambiato: da {previousHealth} a {health}");
        }

        if (health < previousHealth)
        {
            int scoreValue = Score.score;
            int killValue = Enemy.killcounter;
            int deathValue = Death.deathcounter;

            Debug.Log($"Valori: Score={scoreValue}, Kills={killValue}, Deaths={deathValue}");

            int currentScore = scoreValue * 10 + killValue * 15 - deathValue * 10;
            Debug.Log($"Giocatore ha perso una vita! Salvataggio punteggio: {currentScore}");

            try
            {
                if (Ranking.Instance != null)
                {
                    Ranking.Instance.AddScore(currentScore);
                    Debug.Log("Punteggio aggiunto al ranking con successo");
                }
                else
                {
                    Debug.LogError("Ranking.Instance è null! Impossibile salvare il punteggio");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Errore durante il salvataggio del punteggio: {e.Message}\n{e.StackTrace}");
            }
        }

        previousHealth = health;

        switch (health)
        {
            case 3:
                live3.SetActive(true);
                live2.SetActive(false);
                live1.SetActive(false);
                live0.SetActive(false);
                break;
            case 2:
                live3.SetActive(false);
                live2.SetActive(true);
                live1.SetActive(false);
                live0.SetActive(false);
                break;
            case 1:
                live3.SetActive(false);
                live2.SetActive(false);
                live1.SetActive(true);
                live0.SetActive(false);
                break;
            case 0:
                live3.SetActive(false);
                live2.SetActive(false);
                live1.SetActive(false);
                live0.SetActive(true);
                StartCoroutine(DeathPanel());
                StartCoroutine(DeathTime());
                break;
        }
    }

    IEnumerator DeathTime()
    {
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0;
    }

    IEnumerator DeathPanel()
    {
        yield return new WaitForSeconds(0.9f);
        pauseScene.SetActive(true);
    }
}