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
    public GameObject equimentWindow;

    [Header("Weapon Inventory")]
    public GameObject weaponInventorySlotPrefab;
    public Transform weaponInventorySlotParent;
    PlayerInventory playerInventory;
    EquimentWindowUI equimentWindowUI;
    WeaponInventorySlot[] weaponInventorySlots;

    private void Awake()
    {
      equimentWindowUI = FindObjectOfType<EquimentWindowUI>();
    }

    private void Start()
    {
      playerInventory = FindObjectOfType<PlayerInventory>();
      weaponInventorySlots = weaponInventorySlotParent.GetComponentsInChildren<WeaponInventorySlot>();

      if (equimentWindowUI != null)
      {
        equimentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
      }
    }

    public void UpdateUI()
    {
      #region Weapon Inventory Slots
      for (int i = 0; i < weaponInventorySlots.Length; i++)
      {
        int weaponsInInventory = playerInventory.weaponsInventory.Count;

        if (i < weaponsInInventory)
        {
          if (weaponInventorySlots.Length < weaponsInInventory)
          {
            Instantiate(weaponInventorySlotPrefab, weaponInventorySlotParent);
            weaponInventorySlots = weaponInventorySlotParent.GetComponentsInChildren<WeaponInventorySlot>();
          }

          weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
        }
        else
        {
          weaponInventorySlots[i].ClearInventorySlot();
        }
      }
      #endregion

      equimentWindowUI = FindObjectOfType<EquimentWindowUI>();

      if (equimentWindowUI != null)
      {
        equimentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventory);
      }
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
      equimentWindow.SetActive(false);
    }
  }
}
