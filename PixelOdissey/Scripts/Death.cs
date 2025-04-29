using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Death : MonoBehaviour
{
    public GameObject player;
    public Vector2 checkPos;
    private Animator anim;
    private Collider2D _collider2D;
    [SerializeField] private AudioClip audiohurt;
    [SerializeField] private AudioSource audiosource;

    public static bool isDeath = false;
    public static bool isAttack;
    public static int deathcounter = 0;

    private void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        isDeath = false;
        SubscribeToTimer();
    }

    private void Awake()
    {
        isDeath = false;
    }

    void OnDestroy()
    {
        UnsubscribeFromTimer();
    }

    private void SubscribeToTimer()
    {
        if (Timer.Instance != null)
        {
            Timer.Instance.OnTimerEnd += HandleTimerDeath;
            Debug.Log($"{gameObject.name} (Death.cs) si è iscritto all'evento OnTimerEnd.");
        }
        else
        {
            Debug.LogWarning($"Istanza del Timer non trovata in Start() di {gameObject.name} (Death.cs). La morte per timeout non funzionerà.");
        }
    }

    private void UnsubscribeFromTimer()
    {
        if (Timer.Instance != null)
        {
            Timer.Instance.OnTimerEnd -= HandleTimerDeath;
            Debug.Log($"{gameObject.name} (Death.cs) si è disiscritto dall'evento OnTimerEnd.");
        }
    }

    private void HandleTimerDeath()
    {
        if (isDeath) return;

        Debug.Log("Timer scaduto! Il giocatore muore.");
        TriggerPlayerDeath("Timeout");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isDeath) return;

        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collisione con Enemy! Il giocatore muore.");
            TriggerPlayerDeath("Collisione con Nemico");
        }
    }

    public void TriggerPlayerDeath(string reason)
    {
        if (isDeath)
        {
            return;
        }
        isDeath = true;

        Debug.Log($"Morte giocatore attivata ({gameObject.name}). Causa: {reason}");

        if (reason != "Timeout" && Timer.Instance != null && Timer.Instance.IsRunning())
        {
            Timer.Instance.StopTimer();
            Debug.Log("Timer fermato a causa della morte del giocatore.");
        }

        anim.SetTrigger("death");
        isAttack = true;
        HealthSystem.health--;
        deathcounter++;
        _collider2D.enabled = false;
        audiosource.PlayOneShot(audiohurt, 0.5f);
        Invoke("LoadCheckPoint", 1f);
    }

    void LoadCheckPoint()
    {
        Debug.Log($"Respawn giocatore ({gameObject.name}) a {checkPos}");

        isDeath = false;
        isAttack = false;
        _collider2D.enabled = true;
        gameObject.transform.position = checkPos;

        if (Timer.Instance != null)
        {
            Timer.Instance.ResetAndStartTimer();
            Debug.Log("Timer resettato e riavviato dopo il respawn.");
        }
        else
        {
            Debug.LogError("Istanza del Timer non trovata durante il respawn!");
        }
    }
}