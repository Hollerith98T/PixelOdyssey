using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    // --- Singleton ---
    public static Timer Instance { get; private set; }

    [Header("Timer Settings")]
    [SerializeField]
    [Tooltip("Il componente TextMeshProUGUI da aggiornare.")]
    private TextMeshProUGUI timerText;

    [SerializeField]
    [Tooltip("Tempo iniziale in minuti.")]
    private float startTimeInMinutes = 10f;

    public event Action OnTimerEnd;

    private float currentTime;
    private bool timerIsRunning = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Esiste già un'istanza di Timer. Distruggo questa.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        ResetAndStartTimer();
    }

    private void Update()
    {
        if (!timerIsRunning)
            return;

        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            currentTime = Mathf.Max(currentTime, 0f);
            UpdateTimerDisplay();
        }

        if (currentTime <= 0)
        {
            Debug.Log("Timer Finito!");
            timerIsRunning = false;
            UpdateTimerDisplay();
            OnTimerEnd?.Invoke();
        }
    }

    private void UpdateTimerDisplay()
    {
        if (timerText == null)
        {
            if (!timerIsRunning || currentTime == startTimeInMinutes * 60f)
                Debug.LogError("TimerText (TextMeshProUGUI) non è assegnato nell'Inspector!");
            return;
        }

        float displayTime = Mathf.Max(currentTime, 0f);
        int minutes = Mathf.FloorToInt(displayTime / 60);
        int seconds = Mathf.FloorToInt(displayTime % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }


    public void ResetAndStartTimer()
    {
        currentTime = startTimeInMinutes * 60f;
        timerIsRunning = true;
        UpdateTimerDisplay();
        Debug.Log($"Timer Resettato e Avviato. Tempo iniziale: {startTimeInMinutes} minuti.");
    }

    public void StartTimer()
    {
        if (currentTime > 0 && !timerIsRunning)
        {
            timerIsRunning = true;
            Debug.Log("Timer Avviato/Ripreso.");
        }
        else if (timerIsRunning)
        {
            Debug.Log("Timer già in esecuzione.");
        }
        else
        {
            Debug.LogWarning("Impossibile avviare/riprendere il timer: tempo scaduto.");
        }
    }

    public void StopTimer()
    {
        if (timerIsRunning)
        {
            timerIsRunning = false;
            Debug.Log("Timer Fermato.");
        }
    }

    public void ResetTimer()
    {
        timerIsRunning = false;
        currentTime = startTimeInMinutes * 60f;
        UpdateTimerDisplay();
        Debug.Log("Timer Resettato.");
    }

    public void SetStartTime(float newStartTimeMinutes)
    {
        startTimeInMinutes = Mathf.Max(0, newStartTimeMinutes);
        ResetTimer();
        Debug.Log($"Nuovo tempo iniziale impostato a {startTimeInMinutes} minuti. Timer resettato.");
    }

    public float GetCurrentTimeSeconds()
    {
        return currentTime;
    }

    public bool IsRunning()
    {
        return timerIsRunning;
    }
}