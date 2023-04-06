using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public class PlayerManager : MonoBehaviour
  {

    InputHandler inputHandler;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
      inputHandler = GetComponent<InputHandler>();
      animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
      inputHandler.isInteracting = animator.GetBool(AnimationTags.IS_INTERACTING_PARAM);
      inputHandler.rollFlag = false;
      inputHandler.sprintFlag = false;
    }
  }

}

