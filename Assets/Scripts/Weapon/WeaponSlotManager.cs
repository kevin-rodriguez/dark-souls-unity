using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public class WeaponSlotManager : MonoBehaviour
  {
    WeaponHolderSlot leftHandSlot, rightHandSlot;

    DamageCollider leftHandWeaponCollider, rightHandWeaponCollider;

    Animator animator;
    QuickSlotsUI quickSlotsUI;

    private void Awake()
    {
      animator = GetComponent<Animator>();
      quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
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
        quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);

        #region Handle Left Weapon Idle Animation
        if (weaponItem != null)
        {
          animator.CrossFade(weaponItem.Left_Hand_Idle, 0.2f);
        }
        else
        {
          animator.CrossFade(AnimationTags.LEFT_ARM_EMPTY, 0.2f);
        }
        #endregion
      }
      else
      {
        rightHandSlot.LoadWeaponModel(weaponItem);
        LoadRightWeaponDamageCollider();
        quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);

        #region Handle Right Weapon Idle Animation
        if (weaponItem != null)
        {
          animator.CrossFade(weaponItem.Right_Hand_Idle, 0.2f);
        }
        else
        {
          animator.CrossFade(AnimationTags.RIGHT_ARM_EMPTY, 0.2f);
        }
        #endregion
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
