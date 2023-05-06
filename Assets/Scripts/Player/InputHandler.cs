using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KR
{
  public class InputHandler : MonoBehaviour
  {
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;
    public bool b_Input, interact_Input, rb_Input, rt_Input, jump_Input, inventoryInput, lockOnInput;
    public bool d_Pad_Up, d_Pad_Down, d_Pad_Left, d_Pad_Right;
    public bool rollFlag;
    public bool sprintFlag;
    public bool comboFlag;
    public bool inventoryFlag;
    public bool lockOnFlag;
    public float rollInputTimer;

    PlayerControls inputActions;
    PlayerAttacker playerAttacker;
    PlayerInventory playerInventory;
    PlayerManager playerManager;
    UIManager uIManager;

    CameraHandler cameraHandler;
    Vector2 movementInput;
    Vector2 cameraInput;

    private void Awake()
    {
      playerAttacker = GetComponent<PlayerAttacker>();
      playerInventory = GetComponent<PlayerInventory>();
      playerManager = GetComponent<PlayerManager>();
      uIManager = FindObjectOfType<UIManager>();
      cameraHandler = FindObjectOfType<CameraHandler>();
    }

    public void OnEnable()
    {
      if (inputActions == null)
      {
        inputActions = new PlayerControls();
        inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
        inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
        inputActions.PlayerActions.Interact.performed += i => interact_Input = true;
        inputActions.PlayerActions.RB.performed += i => rb_Input = true;
        inputActions.PlayerActions.RT.performed += i => rt_Input = true;
        inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
        inputActions.PlayerActions.Inventory.performed += i => inventoryInput = true;
        inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
        inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
        inputActions.PlayerActions.LockOn.performed += i => lockOnInput = true;
      }

      inputActions.Enable();
    }

    private void OnDisable()
    {
      inputActions.Disable();
    }

    public void TickInput(float delta)
    {
      MoveInput(delta);
      HandleRollInput(delta);
      HandleAttackInput(delta);
      HandleQuickSlotInput();
      HandleInventoryInput();
      HandleLockOnInput();
    }

    private void MoveInput(float delta)
    {
      horizontal = movementInput.x;
      vertical = movementInput.y;
      moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
      mouseX = cameraInput.x;
      mouseY = cameraInput.y;
    }

    private void HandleRollInput(float delta)
    {
      b_Input = inputActions.PlayerActions.Roll.inProgress;
      sprintFlag = b_Input;

      if (b_Input)
      {
        rollInputTimer += delta;
      }
      else
      {
        if (rollInputTimer > 0 && rollInputTimer < 0.5f)
        {
          sprintFlag = false;
          rollFlag = true;
        }

        rollInputTimer = 0;
      }
    }
    private void HandleAttackInput(float delta)
    {
      // RB/RT Handle right hand weapons
      if (rb_Input || rt_Input)
      {
        if (playerManager.canDoCombo)
        {
          comboFlag = true;
          playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
          comboFlag = false;
        }
        else if ((!playerManager.isInteracting))
        {
          if (rb_Input)
          {
            playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
          }
          else if (rt_Input)
          {
            playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
          }
        }
      }
    }

    private void HandleQuickSlotInput()
    {
      if (d_Pad_Right)
      {
        playerInventory.ChangeRightWeapon();
      }

      if (d_Pad_Left)
      {
        playerInventory.ChangeLeftWeapon();
      }
    }

    private void PauseInputs(bool shouldPause)
    {
      if (shouldPause)
      {
        inputActions.PlayerMovement.Disable();
      }
      else
      {
        inputActions.PlayerMovement.Enable();
      }
    }

    private void HandleInventoryInput()
    {
      if (inventoryInput)
      {
        inventoryFlag = !inventoryFlag;

        if (inventoryFlag)
        {
          uIManager.OpenSelectWindow();
          uIManager.UpdateUI();
        }
        else
        {
          uIManager.CloseSelectWindow();
          uIManager.CloseAllInventoryWindows();
        }

        PauseInputs(inventoryFlag);
        uIManager.hudWindow.SetActive(!inventoryFlag);
      }
    }

    private void HandleLockOnInput()
    {
      if (lockOnInput)
      {
        cameraHandler.ClearLockOnTargets();
        lockOnInput = false;

        if (!lockOnFlag)
        {
          cameraHandler.HandleLockOn();

          if (cameraHandler.nearestLockOnTarget != null)
          {
            cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
            lockOnFlag = true;
          }
        }
        else
        {
          lockOnFlag = false;
        }
      }
    }
  }
}

