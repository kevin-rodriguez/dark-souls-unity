using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{

  [CreateAssetMenu(menuName = "Item/Weapon Item")]
  public class WeaponItem : Item
  {
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("Idle Animations")]
    public string Right_Hand_Idle;
    public string Left_Hand_Idle;

    [Header("One Handed Attack Animations")]
    public string OH_Light_Attack_01;
    public string OH_Light_Attack_02;
    public string OH_Heavy_Attack_01;
    public string OH_Heavy_Attack_02;

    [Header("Stamina Costs")]
    public int baseStamina;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;

  }

}
