using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public class WeaponSlotManager : MonoBehaviour
  {
    WeaponHolderSlot leftHandSlot, rightHandSlot;

    DamageCollider leftHandWeaponCollider, rightHandWeaponCollider;

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
        LoadLeftWeaponDamageCollider();
      }
      else
      {
        rightHandSlot.LoadWeaponModel(weaponItem);
        LoadRightWeaponDamageCollider();
      }
    }

    #region Weapon's Damage Collider

    public void LoadLeftWeaponDamageCollider()
    {
      leftHandWeaponCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    public void LoadRightWeaponDamageCollider()
    {
      rightHandWeaponCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    public void OpenRightHandDamageCollider()
    {
      rightHandWeaponCollider.EnableDamageCollider();
    }

    public void OpenLeftHandDamageCollider()
    {
      leftHandWeaponCollider.EnableDamageCollider();
    }

    public void CloseRightHandDamageCollider()
    {
      rightHandWeaponCollider.DisableDamageCollider();
    }

    public void CloseLeftHandDamageCollider()
    {
      leftHandWeaponCollider.DisableDamageCollider();
    }

    #endregion
  }

}
