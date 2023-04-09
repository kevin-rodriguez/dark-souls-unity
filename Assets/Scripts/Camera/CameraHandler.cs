using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{

  public class CameraHandler : MonoBehaviour
  {
    public Transform targetTransform;
    public Transform cameraTransform;
    public Transform cameraPivotTransform;
    private Transform myTransform;
    private Vector3 cameraTransformPosition;
    private LayerMask ignoreLayers;
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


    private void Awake()
    {
      myTransform = transform;
      defaultPosition = cameraTransform.localPosition.z;
      ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
      targetTransform = FindObjectOfType<PlayerManager>().transform;
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
      lookAngle += (mouseXInput * lookSpeed) / delta;
      pivotAngle -= (mouseYInput * pivotSpeed) / delta;

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
  }
}

