using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KR
{
  public class PlayerStats : MonoBehaviour
  {
    const int HEALTH_LEVEL_MULTIPLIER = 10;
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    public HealthBar healthBar;

    AnimatorHandler animatorHandler;

    private void Awake()
    {
      animatorHandler = GetComponentInChildren<AnimatorHandler>();
    }

    void Start()
    {
      maxHealth = SetMaxHealthFromHealthLevel();
      currentHealth = maxHealth;
      healthBar.SetMaxHealth(maxHealth);
    }

    private int SetMaxHealthFromHealthLevel()
    {
      maxHealth = healthLevel * HEALTH_LEVEL_MULTIPLIER;
      return maxHealth;
    }

    public void TakeDamage(int damage)
    {
      currentHealth = currentHealth - damage;

      healthBar.SetCurrentHealth(currentHealth);

      animatorHandler.PlayTargetAnimation(AnimationTags.DAMAGE_LIGHT_ANIMATION, true);

      if (currentHealth <= 0)
      {
        currentHealth = 0;
        animatorHandler.PlayTargetAnimation(AnimationTags.DEATH_ANIMATION, true);
      }
    }
  }

}
