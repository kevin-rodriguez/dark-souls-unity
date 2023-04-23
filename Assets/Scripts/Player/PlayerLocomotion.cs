using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public class PlayerLocomotion : MonoBehaviour
  {
    PlayerManager playerManager;
    Transform cameraObject;
    InputHandler inputHandler;
    public Vector3 moveDirection;

    [HideInInspector]
    public Transform myTransform;
    [HideInInspector]
    public AnimatorHandler animatorHandler;
    public new Rigidbody rigidbody;
    public GameObject normalCamera;

    [Header("Ground & Air Detection Stats")]
    [SerializeField]
    private float groundDetectionRayStartPoint = 0.5f;
    [SerializeField]
    private float minimumDistanceNeededToBeginFall = 1f;
    [SerializeField]
    float groundDirectionRayDistance = 0.2f;
    LayerMask ignoreForGroundCheck;
    public float inAirTimer;

    [Header("Movement Stats")]
    [SerializeField]
    private float movementSpeed = 5;
    [SerializeField]
    private float sprintSpeed = 7;
    [SerializeField]
    private float rotationSpeed = 8;
    [SerializeField]
    private float fallingSpeed = 80;

    void Start()
    {
      playerManager = GetComponent<PlayerManager>();
      rigidbody = GetComponent<Rigidbody>();
      inputHandler = GetComponent<InputHandler>();
      animatorHandler = GetComponentInChildren<AnimatorHandler>();
      cameraObject = Camera.main.transform;
      myTransform = transform;
      animatorHandler.Initialize();

      playerManager.isGrounded = true;
      ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
    }

    #region Movement
    Vector3 normalVector;
    Vector3 targetPosition;

    private void HandleRotation(float delta)
    {
      Vector3 targetDir = Vector3.zero;
      float moveOverride = inputHandler.moveAmount;

      targetDir = cameraObject.forward * inputHandler.vertical;
      targetDir += cameraObject.right * inputHandler.horizontal;

      targetDir.Normalize();
      targetDir.y = 0;

      if (targetDir == Vector3.zero)
        targetDir = myTransform.forward;

      float rs = rotationSpeed;

      Quaternion tr = Quaternion.LookRotation(targetDir);
      Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

      myTransform.rotation = targetRotation;
    }

    public void HandleMovement(float delta)
    {
      if (!inputHandler.rollFlag && !playerManager.isInteracting)
      {
        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float speed = movementSpeed;

        if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5)
        {
          speed = sprintSpeed;
          playerManager.isSprinting = true;
        }
        else
        {
          playerManager.isSprinting = false;
        }

        moveDirection *= speed;

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;

        animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);

        if (animatorHandler.canRotate)
        {
          HandleRotation(delta);
        }
      }

    }

    public void HandleRollAndSprinting(float delta)
    {
      if (!animatorHandler.animator.GetBool(AnimationTags.IS_INTERACTING_PARAM))
      {

        if (inputHandler.rollFlag)
        {
          moveDirection = cameraObject.forward * inputHandler.vertical;
          moveDirection += cameraObject.right * inputHandler.horizontal;


          if (inputHandler.moveAmount > 0)
          {
            animatorHandler.PlayTargetAnimation(AnimationTags.ROLL_ANIMATION, true);
            moveDirection.y = 0;
            Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
            myTransform.rotation = rollRotation;
          }
          else
          {
            //animatorHandler.PlayTargetAnimation(AnimationTags.SHIELD_ANIMATION, true);
            //TODO: Implement block
          }
        }
      }
    }

    public void HandleFalling(float delta, Vector3 moveDirection)
    {
      playerManager.isGrounded = false;
      RaycastHit hit;
      Vector3 origin = myTransform.position;
      origin.y += groundDetectionRayStartPoint;

      bool hitObjectInFrontOfPlayer = Physics.Raycast(origin, myTransform.forward, out hit, 0.4f);

      if (hitObjectInFrontOfPlayer)
        moveDirection = Vector3.zero;

      if (playerManager.isInAir)
      {
        rigidbody.AddForce(Vector3.down * fallingSpeed);
        // Small push on edge to prevent being stuck on it
        rigidbody.AddForce(moveDirection * fallingSpeed / 10f);
      }

      Vector3 direction = moveDirection;
      direction.Normalize();
      origin = origin + direction * groundDirectionRayDistance;

      targetPosition = myTransform.position;

      Debug.DrawRay(origin, Vector3.down * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);

      bool hitTheGround = Physics.Raycast(origin, Vector3.down, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck);

      if (hitTheGround)
      {
        normalVector = hit.normal;
        playerManager.isGrounded = true;
        targetPosition.y = hit.point.y;

        if (playerManager.isInAir)
        {
          if (inAirTimer > 0.5f)
          {
            animatorHandler.PlayTargetAnimation(AnimationTags.LAND_ANIMATION, true);
          }
          else
          {
            animatorHandler.PlayTargetAnimation(AnimationTags.EMPTY, false);
          }

          playerManager.isInAir = false;
        }
      }
      else
      {
        if (playerManager.isGrounded)
        {
          playerManager.isGrounded = false;
        }
        if (!playerManager.isInAir)
        {
          if (!playerManager.isInteracting)
          {
            animatorHandler.PlayTargetAnimation(AnimationTags.FALLING_ANIMATION, true);
          }

          Vector3 velocity = rigidbody.velocity;
          velocity.Normalize();
          rigidbody.velocity = velocity * (movementSpeed / 2);
          playerManager.isInAir = true;
        }
      }

      if (playerManager.isGrounded)
      {
        if (playerManager.isInteracting || inputHandler.moveAmount > 0)
        {
          myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
        }
        else
        {
          myTransform.position = targetPosition;
        }
      }

    }

    public void HandleJumping()
    {
      if (!playerManager.isInteracting)
      {

        if (inputHandler.jump_Input)
        {
          string jumpAnimation = inputHandler.moveAmount > 0 ?
            AnimationTags.JUMP_RUNNING_ANIMATION :
            AnimationTags.JUMP_STANDING_ANIMATION;
          moveDirection = cameraObject.forward * inputHandler.vertical;
          moveDirection += cameraObject.right * inputHandler.horizontal;
          animatorHandler.PlayTargetAnimation(jumpAnimation, true);
          moveDirection.y = 0;

          Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
          myTransform.rotation = jumpRotation;
        }
      }
    }
    #endregion

  }

}
