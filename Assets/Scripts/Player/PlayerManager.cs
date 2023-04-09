using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public class PlayerManager : MonoBehaviour
  {

    CameraHandler cameraHandler;
    PlayerLocomotion playerLocomotion;
    InputHandler inputHandler;
    Animator animator;

    [Header("Player Flags")]
    public bool isInteracting;
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;

    private void Awake()
    {
      cameraHandler = FindObjectOfType<CameraHandler>();
    }

    void Start()
    {
      inputHandler = GetComponent<InputHandler>();
      animator = GetComponentInChildren<Animator>();
      playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    void Update()
    {
      float delta = Time.deltaTime;

      isInteracting = animator.GetBool(AnimationTags.IS_INTERACTING_PARAM);

      inputHandler.TickInput(delta);

      playerLocomotion.HandleMovement(delta);
      playerLocomotion.HandleRollAndSprinting(delta);
      playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
    }

    private void LateUpdate()
    {
      float delta = Time.deltaTime;

      HandleCamera(delta);

      inputHandler.rollFlag = false;
      inputHandler.sprintFlag = false;
      inputHandler.rb_Input = false;
      inputHandler.rt_Input = false;

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


  }

}

