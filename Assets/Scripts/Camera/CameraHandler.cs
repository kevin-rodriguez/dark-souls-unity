using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{

  public class CameraHandler : MonoBehaviour
  {
    InputHandler inputHandler;
    public Transform targetTransform;
    public Transform cameraTransform;
    public Transform cameraPivotTransform;
    public LayerMask ignoreLayers;
    private Transform myTransform;
    private Vector3 cameraTransformPosition;
    private Vector3 cameraFollowVelocity = Vector3.zero;

    public float lookSpeed = 0.1f;
    public float followSpeed = 0.1f;
    public float pivotSpeed = 0.03f;

    private float targetPosition;

    private float defaultPosition;
    private float lookAngle;
    private float pivotAngle;
    public float minimumPivot = -35;
    public float maximumPivot = 35;
    public float cameraSphereRadius = 0.2f;
    public float cameraCollisionOffset = 0.2f;
    public float minimumCollisionOffset = 0.2f;

    List<CharacterManager> availableTargets = new List<CharacterManager>();
    public Transform nearestLockOnTarget;
    public Transform currentLockOnTarget;
    public float maximumLockOnDistance = 30;

    private void Awake()
    {
      myTransform = transform;
      defaultPosition = cameraTransform.localPosition.z;
      ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
      targetTransform = FindObjectOfType<PlayerManager>().transform;
      inputHandler = FindObjectOfType<InputHandler>();
    }

    public void FollowTarget(float delta)
    {
      Vector3 targetPosition = Vector3.SmoothDamp
                                (myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
      myTransform.position = targetPosition;

      HandleCameraCollisions(delta);
    }

    public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
    {
      if (!inputHandler.lockOnInput && currentLockOnTarget == null)
      {
        lookAngle += mouseXInput * lookSpeed * delta;
        pivotAngle -= mouseYInput * pivotSpeed * delta;

        // Add limits to camera movement
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        myTransform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;

        targetRotation = Quaternion.Euler(rotation);
        cameraPivotTransform.localRotation = targetRotation;
      }
      else
      {
        float velocity = 0;

        Vector3 direction = currentLockOnTarget.position - transform.position;
        direction.Normalize();
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;

        direction = currentLockOnTarget.position - cameraPivotTransform.position;
        direction.Normalize();

        // Force camera rotation to target
        targetRotation = Quaternion.LookRotation(direction);
        Vector3 eulerAngles = targetRotation.eulerAngles;
        eulerAngles.y = 0;
        cameraPivotTransform.localEulerAngles = eulerAngles;
      }
    }

    private void HandleCameraCollisions(float delta)
    {
      targetPosition = defaultPosition;
      RaycastHit hit;
      Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
      direction.Normalize();

      // Check if camera collides with any object
      if (Physics.SphereCast(cameraPivotTransform.position,
                            cameraSphereRadius, direction,
                            out hit,
                            Mathf.Abs(targetPosition),
                            ignoreLayers))
      {
        float distance = Vector3.Distance(cameraPivotTransform.position, hit.point);
        targetPosition = -(distance - cameraCollisionOffset);
      }

      // Re-position camera
      if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
      {
        targetPosition = -minimumCollisionOffset;
      }
      //cameraTransformPosition.z = Vector3.SmoothDamp(cameraTransform.localPosition.z, targetPosition, delta/0.2f);
      cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
      cameraTransform.localPosition = cameraTransformPosition;
    }

    public void HandleLockOn()
    {
      float shortestDistanceToTarget = Mathf.Infinity;
      float collidingDistance = 26;

      Collider[] colliders = Physics.OverlapSphere(targetTransform.position, collidingDistance);

      foreach (Collider collider in colliders)
      {
        CharacterManager character = collider.GetComponent<CharacterManager>();

        if (character != null)
        {
          Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
          float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
          float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);

          bool isPlayerTarget = character.gameObject == targetTransform.gameObject;
          bool inViewableAngle = viewableAngle > -50 && viewableAngle < 50;
          bool inDistance = distanceFromTarget <= maximumLockOnDistance;

          if (!isPlayerTarget && inViewableAngle && inDistance)
          {
            availableTargets.Add(character);
          }

          foreach (CharacterManager availableTarget in availableTargets)
          {
            float distanceFromAvailableTarget = Vector3.Distance(targetTransform.position, availableTarget.transform.position);

            if (distanceFromAvailableTarget < shortestDistanceToTarget)
            {
              shortestDistanceToTarget = distanceFromAvailableTarget;
              nearestLockOnTarget = availableTarget.lockOnTransform;
            }
          }

        }
      }
    }

    public void ClearLockOnTargets()
    {
      availableTargets.Clear();
      nearestLockOnTarget = null;
      currentLockOnTarget = null;
    }

  }
}

