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

    const int STAMINA_LEVEL_MULTIPLIER = 10;
    public int staminaLevel = 10;
    public int maxStamina;
    public int currentStamina;

    public HealthBar healthBar;
    public StaminaBar staminaBar;

    AnimatorHandler animatorHandler;

    private void Awake()
    {
      animatorHandler = GetComponentInChildren<AnimatorHandler>();
      healthBar = FindObjectOfType<HealthBar>();
      staminaBar = FindObjectOfType<StaminaBar>();
    }

    void Start()
    {
      maxHealth = SetMaxHealthFromHealthLevel();
      currentHealth = maxHealth;
      healthBar.SetMaxHealth(maxHealth);

      maxStamina = SetMaxStaminaFromStaminaLevel();
      currentStamina = maxStamina;
      staminaBar.SetMaxStamina(maxStamina);
    }

    private int SetMaxHealthFromHealthLevel()
    {
      maxHealth = healthLevel * HEALTH_LEVEL_MULTIPLIER;
      return maxHealth;
    }

    private int SetMaxStaminaFromStaminaLevel()
    {
      maxStamina = staminaLevel * STAMINA_LEVEL_MULTIPLIER;
      return maxStamina;
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

    public void TakeStamina(int stamina)
    {
      currentStamina -= stamina;
      staminaBar.SetCurrentStamina(currentStamina);
    }
  }

}
