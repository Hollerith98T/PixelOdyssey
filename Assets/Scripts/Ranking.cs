using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class Ranking : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private static Ranking _instance;
    [SerializeField] private GameObject rankingPanel;

    public static Ranking Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<Ranking>(); // Trova la classe Ranking nella scenas
                if (_instance == null)
                {
                    Debug.LogError("Componente Ranking non trovato nella scena.");
                }
            }
            return _instance;
        }
    }

    private List<ScoreEntry> scores = new List<ScoreEntry>();
    private const int MaxScores = 5;

    void Start()
    {
        LoadRanking();
        //AddTestScores();
    }

    private void AddTestScores()
    {
        // Solo per test - aggiungi alcuni punteggi di esempio se non ce ne sono
        if (scores.Count == 0)
        {
            Debug.Log("Aggiunta punteggi di test");
            AddScore(1000);
            AddScore(750);
            AddScore(500);
            Debug.Log($"Punteggi di test aggiunti: {scores.Count}");
        }
    }

    public void AddScore(int score)
    {
        Debug.Log($"AddScore chiamato con punteggio: {score}");
        InternalAddScore(score, DateTime.Now);
    }

    public void AddScore(int currentScoreValue, int killCount, int deathCount)
    {
        int calculatedScore = currentScoreValue * 10 + killCount * 15 - deathCount * 10;
        Debug.Log($"AddScore chiamato con valori: Score={currentScoreValue}, Kills={killCount}, Deaths={deathCount}. Punteggio calcolato: {calculatedScore}");
        InternalAddScore(calculatedScore, DateTime.Now);
    }

    private void InternalAddScore(int scoreToAdd, DateTime timestamp)
    {
        ScoreEntry newScore = new ScoreEntry(scoreToAdd, timestamp);
        scores.Add(newScore);
        scores.Sort((x, y) => y.Score.CompareTo(x.Score));
        if (scores.Count > MaxScores)
        {
            scores.RemoveAt(scores.Count - 1);
        }
        SaveRanking();

        // Forza PlayerPrefs.Save() pi� volte per sicurezza
        PlayerPrefs.Save();

        Debug.Log($"Punteggi dopo AddScore: {scores.Count}");
        foreach (var entry in scores)
        {
            Debug.Log($"- {entry.Score} - {entry.Timestamp}");
        }
    }

    void DisplayTopScores()
    {
        Debug.Log("DisplayTopScores chiamato");

        if (scoreText == null)
        {
            Debug.LogError("scoreText � null! Controlla di averlo assegnato nell'Inspector");
            return;
        }

        scoreText.text = "";
        // Usa direttamente scores invece di passare per Instance
        var topScores = new List<ScoreEntry>(scores);

        Debug.Log($"Numero di punteggi da visualizzare: {topScores.Count}");

        foreach (var score in topScores)
        {
            scoreText.text += $"{score.Score} - {score.Timestamp.ToString("yyyy-MM-dd HH:mm")}\n";
            Debug.Log($"Aggiunto punteggio: {score.Score} - {score.Timestamp}");
        }
    }
    // Ottieni i migliori punteggi
    public List<ScoreEntry> GetTopScores()
    {
        return new List<ScoreEntry>(scores);
    }

    // Salva i punteggi in PlayerPrefs
    private void SaveRanking()
    {
        for (int i = 0; i < scores.Count; i++)
        {
            Debug.Log($"Salvataggio punteggio {i + 1}: {scores[i].Score} - {scores[i].Timestamp}");
            PlayerPrefs.SetInt($"score_{i + 1}", scores[i].Score);
            PlayerPrefs.SetString($"timestamp_{i + 1}", scores[i].Timestamp.ToString("yyyy-MM-dd HH:mm"));
        }
        PlayerPrefs.Save();
    }

    private void LoadRanking()
    {
        scores.Clear();
        Debug.Log("Inizio caricamento ranking");

        for (int i = 0; i < MaxScores; i++)
        {
            if (PlayerPrefs.HasKey($"score_{i + 1}"))
            {
                int score = PlayerPrefs.GetInt($"score_{i + 1}");
                string timestampStr = PlayerPrefs.GetString($"timestamp_{i + 1}");
                DateTime timestamp = DateTime.Parse(timestampStr);
                scores.Add(new ScoreEntry(score, timestamp));
                Debug.Log($"Caricato punteggio {i + 1}: {score} - {timestampStr}");
            }
            else
            {
                Debug.Log($"Nessun punteggio trovato per posizione {i + 1}");
            }
        }

        // Ordina i punteggi
        scores.Sort((x, y) => y.Score.CompareTo(x.Score));
        Debug.Log($"Totale punteggi caricati: {scores.Count}");
    }

    public void EnableRankingPanel()
    {
        if (rankingPanel != null)
        {
            rankingPanel.SetActive(true);
            DisplayTopScores();
        }
        else
        {
            Debug.LogError("Il pannello della classifica non � stato assegnato nell'Inspector.");
        }
    }

    public void DisableRankingPanel()
    {
        if (rankingPanel != null)
        {
            rankingPanel.SetActive(false);
        }
    }
}

[Serializable]
public class ScoreEntry
{
    public int Score;
    public DateTime Timestamp;

    public ScoreEntry(int score, DateTime timestamp)
    {
        Score = score;
        Timestamp = timestamp;
    }
}
