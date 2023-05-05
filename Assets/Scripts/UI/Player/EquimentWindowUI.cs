using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public class EquimentWindowUI : MonoBehaviour
  {
    public bool rightHandSlot01Selected;
    public bool rightHandSlot02Selected;
    public bool leftHandSlot01Selected;
    public bool leftHandSlot02Selected;

    HandEquimentSlotUI[] handEquimentSlotsUI;

    private void Start()
    {
      handEquimentSlotsUI = GetComponentsInChildren<HandEquimentSlotUI>();
    }

    public void LoadWeaponsOnEquipmentScreen(PlayerInventory playerInventory)
    {
      for (int i = 0; i < handEquimentSlotsUI.Length; i++)
      {
        if (handEquimentSlotsUI[i].rightHandSlot01)
        {
          handEquimentSlotsUI[i].AddItem(playerInventory.weaponsInRightHandSlots[0]);
        }
        else if (handEquimentSlotsUI[i].rightHandSlot02)
        {

          handEquimentSlotsUI[i].AddItem(playerInventory.weaponsInRightHandSlots[1]);
        }
        else if (handEquimentSlotsUI[i].leftHandSlot01)
        {
          handEquimentSlotsUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[0]);
        }
        else
        {
          handEquimentSlotsUI[i].AddItem(playerInventory.weaponsInLeftHandSlots[1]);
        }
      }
    }
    public void SelectRightHandSlot01()
    {
      rightHandSlot01Selected = true;
    }

    public void SelectRightHandSlot02()
    {
      rightHandSlot02Selected = true;
    }

    public void SelectLeftHandSlot01()
    {
      rightHandSlot01Selected = true;
    }

    public void SelectLeftHandSlot02()
    {
      rightHandSlot02Selected = true;
    }

  }

}
