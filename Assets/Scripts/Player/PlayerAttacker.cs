using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public class PlayerAttacker : MonoBehaviour
  {

    AnimatorHandler animatorHandler;
    InputHandler inputHandler;
    WeaponSlotManager weaponSlotManager;
    public string lastAttack;


    private void Awake()
    {
      animatorHandler = GetComponentInChildren<AnimatorHandler>();
      weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
      inputHandler = GetComponent<InputHandler>();
    }

    public void HandleWeaponCombo(WeaponItem weapon)
    {
      if (inputHandler.comboFlag)
      {
        animatorHandler.animator.SetBool(AnimationTags.CAN_DO_COMBO_PARAM, false);

        if (lastAttack == weapon.OH_Light_Attack_01)
        {
          animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_02, true);
        }
        else if (lastAttack == weapon.OH_Heavy_Attack_01)
        {
          animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_02, true);
        }
      }
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
      weaponSlotManager.attackingWeapon = weapon;
      animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_01, true);
      lastAttack = weapon.OH_Light_Attack_01;
    }
    public void HandleHeavyAttack(WeaponItem weapon)
    {
      weaponSlotManager.attackingWeapon = weapon;
      animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_01, true);
      lastAttack = weapon.OH_Heavy_Attack_01;
    }
  }

}
