using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace KR
{
  public class WeaponPickUp : Interactable
  {
    public WeaponItem weapon;
    public override void Interact(PlayerManager playerManager)
    {
      base.Interact(playerManager);

      PickUpItem(playerManager);
    }

    private void PickUpItem(PlayerManager playerManager)
    {
      PlayerInventory playerInventory;
      PlayerLocomotion playerLocomotion;
      AnimatorHandler animatorHandler;

      playerInventory = playerManager.GetComponent<PlayerInventory>();
      playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
      animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();

      playerManager.itemInteractableGameObject.GetComponentInChildren<TMP_Text>().text = weapon.itemName;
      playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
      playerManager.itemInteractableGameObject.SetActive(true);

      playerLocomotion.rigidbody.velocity = Vector3.zero;
      animatorHandler.PlayTargetAnimation(AnimationTags.PICK_UP_ITEM_ANIMATION, true);
      playerInventory.weaponsInventory.Add(weapon);
      Destroy(gameObject);
    }

  }
}
