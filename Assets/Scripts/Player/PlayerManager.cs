using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public class PlayerManager : CharacterManager
  {

    CameraHandler cameraHandler;
    PlayerLocomotion playerLocomotion;
    InputHandler inputHandler;
    Animator animator;
    InteractableUI interactableUI;
    public GameObject interactableUIGameObject;
    public GameObject itemInteractableGameObject;

    [Header("Player Flags")]
    public bool isInteracting;
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;
    public bool canDoCombo;

    [Header("Interaction")]
    public float interactionRadius = 0.5f;
    public float interactionMaxDistance = 1f;

    private void Awake()
    {
      cameraHandler = FindObjectOfType<CameraHandler>();
    }

    void Start()
    {
      inputHandler = GetComponent<InputHandler>();
      animator = GetComponentInChildren<Animator>();
      playerLocomotion = GetComponent<PlayerLocomotion>();
      interactableUI = FindObjectOfType<InteractableUI>();
    }

    void Update()
    {
      float delta = Time.deltaTime;

      isInteracting = animator.GetBool(AnimationTags.IS_INTERACTING_PARAM);
      canDoCombo = animator.GetBool(AnimationTags.CAN_DO_COMBO_PARAM);
      animator.SetBool(AnimationTags.IS_IN_AIR_PARAM, isInAir);

      inputHandler.TickInput(delta);

      playerLocomotion.HandleRollAndSprinting(delta);
      playerLocomotion.HandleJumping();

      CheckForInteractableObject();
    }

    private void FixedUpdate()
    {
      float delta = Time.deltaTime;
      playerLocomotion.HandleMovement(delta);
      playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
    }

    private void LateUpdate()
    {
      float delta = Time.deltaTime;

      HandleCamera(delta);

      inputHandler.rollFlag = false;
      inputHandler.rb_Input = false;
      inputHandler.rt_Input = false;
      inputHandler.d_Pad_Up = false;
      inputHandler.d_Pad_Down = false;
      inputHandler.d_Pad_Right = false;
      inputHandler.d_Pad_Left = false;
      inputHandler.interact_Input = false;
      inputHandler.jump_Input = false;
      inputHandler.inventoryInput = false;

      if (isInAir)
      {
        playerLocomotion.inAirTimer += delta;
      }
    }

    void HandleCamera(float delta)
    {
      if (cameraHandler != null)
      {
        cameraHandler.FollowTarget(delta);
        cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
      }
    }

    public void CheckForInteractableObject()
    {
      RaycastHit hit;

      if (Physics.SphereCast(transform.position, interactionRadius, transform.forward, out hit, interactionMaxDistance, cameraHandler.ignoreLayers))
      {
        if (hit.collider.CompareTag(Tags.INTERACTABLE_TAG))
        {
          Interactable interactableObject = hit.collider.GetComponent<Interactable>();

          if (interactableObject != null)
          {
            string interactableText = interactableObject.interactableText;
            interactableUI.interactableText.text = interactableText;
            interactableUIGameObject.SetActive(true);

            if (inputHandler.interact_Input)
            {
              hit.collider.GetComponent<Interactable>().Interact(this);
            }
          }
        }
      }
      else
      {
        if (interactableUIGameObject != null)
          interactableUIGameObject.SetActive(false);

        if (itemInteractableGameObject != null && inputHandler.interact_Input)
          itemInteractableGameObject.SetActive(false);
      }
    }
  }

}

