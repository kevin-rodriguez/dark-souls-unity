using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public class WeaponSlotManager : MonoBehaviour
  {
    WeaponHolderSlot leftHandSlot, rightHandSlot;

    private void Awake()
    {
      WeaponHolderSlot[] weaponHolderSlot = GetComponentsInChildren<WeaponHolderSlot>();

      foreach (WeaponHolderSlot weaponSlot in weaponHolderSlot)
      {
        if (weaponSlot.isLeftHandSlot)
        {
          leftHandSlot = weaponSlot;
        }
        else if (weaponSlot.isRightHandSlot)
        {
          rightHandSlot = weaponSlot;
        }
      }
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
      if (isLeft)
      {
        leftHandSlot.LoadWeaponModel(weaponItem);
      }
      else
      {
        rightHandSlot.LoadWeaponModel(weaponItem);
      }
    }
  }

}
