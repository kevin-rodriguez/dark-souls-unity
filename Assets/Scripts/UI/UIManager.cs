using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{

  public class UIManager : MonoBehaviour
  {
    [Header("UI Windows")]
    public GameObject selectWindow;
    public GameObject hudWindow;
    public GameObject weaponInventoryWindow;

    [Header("Weapon Inventory")]
    public GameObject weaponInventorySlotPrefab;
    public Transform weaponInventorySlotParent;
    PlayerInventory playerInventory;
    WeaponInventorySlot[] weaponInventorySlots;

    private void Start()
    {
      playerInventory = FindObjectOfType<PlayerInventory>();
      weaponInventorySlots = weaponInventorySlotParent.GetComponentsInChildren<WeaponInventorySlot>();
    }

    public void UpdateUI()
    {
      print(weaponInventorySlots.Length);
      #region Weapon Inventory Slots
      for (int i = 0; i < weaponInventorySlots.Length; i++)
      {
        int weaponsInInventory = playerInventory.weaponsInventory.Count;

        print(weaponsInInventory);

        if (i < weaponsInInventory)
        {
          if (weaponInventorySlots.Length < weaponsInInventory)
          {
            Instantiate(weaponInventorySlotPrefab, weaponInventorySlotParent);
            weaponInventorySlots = weaponInventorySlotParent.GetComponentsInChildren<WeaponInventorySlot>();
          }

          print(playerInventory.weaponsInventory[i]);
          weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
        }
        else
        {
          weaponInventorySlots[i].ClearInventorySlot();
        }
      }
      #endregion
    }

    public void OpenSelectWindow()
    {
      selectWindow.SetActive(true);
    }
    public void CloseSelectWindow()
    {
      selectWindow.SetActive(false);
    }

    public void CloseAllInventoryWindows()
    {
      weaponInventoryWindow.SetActive(false);
    }
  }
}
