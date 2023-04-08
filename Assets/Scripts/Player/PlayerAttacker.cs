using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public class PlayerAttacker : MonoBehaviour
  {

    AnimatorHandler animatorHandler;


    private void Awake()
    {
      animatorHandler = GetComponentInChildren<AnimatorHandler>();
    }

    public void HandleLightAttack(WeaponItem weapon)
    {
      animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack, true);
    }
    public void HandleHeavyAttack(WeaponItem weapon)
    {
      animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack, true);
    }
  }

}
