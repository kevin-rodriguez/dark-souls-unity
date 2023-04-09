using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public class EnemyStats : MonoBehaviour
  {

    const int HEALTH_LEVEL_MULTIPLIER = 10;
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    Animator animator;

    private void Awake()
    {
      animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
      maxHealth = SetMaxHealthFromHealthLevel();
      currentHealth = maxHealth;
    }

    private int SetMaxHealthFromHealthLevel()
    {
      maxHealth = healthLevel * HEALTH_LEVEL_MULTIPLIER;
      return maxHealth;
    }

    public void TakeDamage(int damage)
    {
      currentHealth = currentHealth - damage;

      animator.Play(AnimationTags.DAMAGE_LIGHT_ANIMATION);

      if (currentHealth <= 0)
      {
        currentHealth = 0;
        animator.Play(AnimationTags.DEATH_ANIMATION);
      }
    }
  }


}
