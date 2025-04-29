using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    private Animator anim;
    [SerializeField] private AudioClip audioDeadEnemy;
    [SerializeField] private AudioClip audioAttackEnemy;
    [SerializeField] private AudioSource audiosourceEnemy;
    public static bool isEnemyDeath = false;
    public static int killcounter = 0;
    private bool attack;
    public Animator Animator => anim;
    public AudioClip DeathSound { get => audioDeadEnemy; set => audioDeadEnemy = value; }
    public AudioClip AttackSound { get => audioAttackEnemy; set => audioAttackEnemy = value; }

    public void TakeDamage(float amount)
    {
        if (amount > 0)
        {
            Die();
        }
    }

    public void Die()
    {
        audiosourceEnemy.PlayOneShot(audioDeadEnemy, 0.5f);
        anim.SetTrigger("enemyDeath");
        isEnemyDeath = true;
        Destroy(gameObject, 1f);
        killcounter += 1;
    }

    public void Attack(GameObject target)
    {
        if (target != null && audioAttackEnemy != null && !attack)
        {
            audiosourceEnemy.PlayOneShot(audioAttackEnemy, 0.5f);
            anim.SetBool("enemyAttack", true);
            attack = true;
        }
        else if (!Death.isAttack)
        {
            anim.SetBool("enemyAttack", false);
            attack = false;
        }
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Death.isAttack && attack)
        {
            anim.SetBool("enemyAttack", true);
        }
        else
        {
            anim.SetBool("enemyAttack", false);
            attack = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TakeDamage(1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && audioAttackEnemy != null)
        {
            Attack(other.gameObject);
        }
    }
}