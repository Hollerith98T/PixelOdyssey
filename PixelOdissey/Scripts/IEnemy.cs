using UnityEngine;

public interface IEnemy
{
    Animator Animator { get; }
    AudioClip DeathSound { get; set; }
    AudioClip AttackSound { get; set; }

    void TakeDamage(float amount);
    void Die();
    void Attack(GameObject target);
}