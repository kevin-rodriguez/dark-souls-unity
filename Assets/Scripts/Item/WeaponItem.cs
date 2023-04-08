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

    [Header("One Handed Attack Animations")]
    public string OH_Light_Attack;
    public string OH_Heavy_Attack;
  }

}
