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

    private void Awake()
    {
      cameraHandler = CameraHandler.singleton;
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
    }

    private void LateUpdate()
    {
      float delta = Time.deltaTime;

      if (cameraHandler != null)
      {
        cameraHandler.FollowTarget(delta);
        cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
      }

      inputHandler.rollFlag = false;
      inputHandler.sprintFlag = false;
      isSprinting = inputHandler.bInput;
    }
  }

}

