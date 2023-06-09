using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public class AnimatorHandler : MonoBehaviour
  {
    PlayerManager playerManager;
    public Animator animator;
    InputHandler inputHandler;
    PlayerLocomotion playerLocomotion;
    int vertical;
    int horizontal;
    public bool canRotate;

    public void Initialize()
    {
      playerManager = GetComponentInParent<PlayerManager>();
      animator = GetComponent<Animator>();
      inputHandler = GetComponentInParent<InputHandler>();
      playerLocomotion = GetComponentInParent<PlayerLocomotion>();
      vertical = Animator.StringToHash(AnimationTags.VERTICAL);
      horizontal = Animator.StringToHash(AnimationTags.HORIZONTAL);
    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
    {
      #region Vertical
      float v = 0;

      if (verticalMovement > 0 && verticalMovement < 0.55f)
      {
        v = 0.5f;
      }
      else if (verticalMovement > 0.55f)
      {
        v = 1;
      }
      else if (verticalMovement < 0 && verticalMovement > -0.55f)
      {
        v = -0.5f;
      }
      else if (verticalMovement < -0.55f)
      {
        v = -1;
      }
      else
      {
        v = 0;
      }
      #endregion

      #region Horizontal
      float h = 0;
      if (horizontalMovement > 0 && horizontalMovement < 0.55f)
      {
        h = 0.5f;
      }
      else if (horizontalMovement > 0.55f)
      {
        h = 1;
      }
      else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
      {
        h = -0.5f;
      }
      else if (horizontalMovement < -0.55f)
      {
        h = -1;
      }
      else
      {
        h = 0;
      }

      if (isSprinting)
      {
        v = 2;
        h = horizontalMovement;
      }

      #endregion

      animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
      animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

    public void PlayTargetAnimation(string targetAnimator, bool isInteracting)
    {
      animator.applyRootMotion = isInteracting;
      animator.SetBool(AnimationTags.IS_INTERACTING_PARAM, isInteracting);
      animator.CrossFade(targetAnimator, 0.2f);
    }
    public void CanRotate()
    {
      canRotate = true;
    }

    public void StopRotation()
    {
      canRotate = false;
    }

    public void EnableCombo()
    {
      animator.SetBool(AnimationTags.CAN_DO_COMBO_PARAM, true);
    }

    public void DisableCombo()
    {
      animator.SetBool(AnimationTags.CAN_DO_COMBO_PARAM, false);
    }

    private void OnAnimatorMove()
    {
      if (playerManager.isInteracting)
      {
        float delta = Time.deltaTime;

        playerLocomotion.rigidbody.drag = 0;
        Vector3 deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        playerLocomotion.rigidbody.velocity = velocity;
      }
    }

  }

}

