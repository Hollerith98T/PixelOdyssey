using UnityEngine;
using TMPro; // Assicurati di importare il namespace TextMeshPro
using System; // Necessario per System.Action

public class Timer : MonoBehaviour
{
    // --- Singleton Implementation ---
    public static Timer Instance { get; private set; }

    // --- Inspector Variables ---
    [Header("Timer Settings")]
    [SerializeField]
    [Tooltip("Il componente TextMeshProUGUI da aggiornare.")]
    private TextMeshProUGUI timerText;

    [SerializeField]
    [Tooltip("Tempo iniziale in minuti.")]
    private float startTimeInMinutes = 10f;

    // --- Event ---
    /// <summary>
    /// Evento che viene invocato quando il timer raggiunge lo zero.
    /// </summary>
    public event Action OnTimerEnd;

    // --- Private Variables ---
    private float currentTime;      // Tempo corrente in secondi
    private bool timerIsRunning = false;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Esiste già un'istanza di Timer. Distruggo questa.");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            // Opzionale: DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        // All'avvio, imposta il tempo iniziale sul display ma non avvia il timer.
        // Chiama ResetTimer() invece di ResetAndStartTimer() per default.
        ResetAndStartTimer();
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                // Clamp a zero per evitare numeri negativi minimi prima dello stop
                currentTime = Mathf.Max(currentTime, 0f);
                UpdateTimerDisplay();
            }

            // Controlla se il tempo è appena scaduto in questo frame
            if (currentTime <= 0)
            {
                Debug.Log("Timer Finito!");
                timerIsRunning = false;
                UpdateTimerDisplay(); // Assicura che venga visualizzato 00:00
                // Invoca l'evento per notificare gli altri script
                OnTimerEnd?.Invoke();
            }
        }
    }

    private void UpdateTimerDisplay()
    {
        if (timerText == null)
        {
            // Non loggare continuamente l'errore se il timer è in esecuzione
            if (!timerIsRunning || currentTime == startTimeInMinutes * 60f) // Logga solo all'inizio o se non assegnato
                Debug.LogError("TimerText (TextMeshProUGUI) non è assegnato nell'Inspector!");
            return;
        }

        float displayTime = Mathf.Max(currentTime, 0f); // Assicura che non venga mai visualizzato un tempo negativo
        float minutes = Mathf.FloorToInt(displayTime / 60);
        float seconds = Mathf.FloorToInt(displayTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // --- Metodi Pubblici per Controllare il Timer ---

    /// <summary>
    /// Resetta il timer al suo valore iniziale e lo avvia immediatamente.
    /// Ideale per essere chiamato all'inizio di un livello.
    /// </summary>
    public void ResetAndStartTimer()
    {
        currentTime = startTimeInMinutes * 60f; // Resetta il tempo
        timerIsRunning = true;                  // Avvia il conteggio
        UpdateTimerDisplay();                   // Aggiorna subito il display
        Debug.Log($"Timer Resettato e Avviato. Tempo iniziale: {startTimeInMinutes} minuti.");
    }

    /// <summary>
    /// Avvia o riprende il conto alla rovescia, se c'è tempo rimasto.
    /// </summary>
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
        else // currentTime <= 0
        {
            Debug.LogWarning("Impossibile avviare/riprendere il timer: tempo scaduto.");
        }
    }

    /// <summary>
    /// Ferma (mette in pausa) il conto alla rovescia.
    /// </summary>
    public void StopTimer()
    {
        if (timerIsRunning)
        {
            timerIsRunning = false;
            Debug.Log("Timer Fermato.");
        }
    }

    /// <summary>
    /// Resetta il timer al suo valore iniziale e lo ferma. Aggiorna il display.
    /// </summary>
    public void ResetTimer()
    {
        timerIsRunning = false; // Assicura che sia fermo
        currentTime = startTimeInMinutes * 60f;
        UpdateTimerDisplay();
        Debug.Log("Timer Resettato.");
    }

    /// <summary>
    /// Imposta un nuovo tempo di inizio (in minuti) e resetta il timer (lo ferma).
    /// </summary>
    /// <param name="newStartTimeMinutes">Nuovo tempo iniziale in minuti.</param>
    public void SetStartTime(float newStartTimeMinutes)
    {
        startTimeInMinutes = Mathf.Max(0, newStartTimeMinutes); // Assicura che non sia negativo
        ResetTimer(); // Resettare imposta il nuovo tempo e ferma il timer
        Debug.Log($"Nuovo tempo iniziale impostato a {startTimeInMinutes} minuti. Timer resettato.");
    }

    /// <summary>
    /// Restituisce il tempo rimanente in secondi.
    /// </summary>
    public float GetCurrentTimeSeconds()
    {
        return currentTime;
    }

    /// <summary>
    /// Restituisce true se il timer è attualmente in esecuzione.
    /// </summary>
    public bool IsRunning()
    {
        return timerIsRunning;
    }
}