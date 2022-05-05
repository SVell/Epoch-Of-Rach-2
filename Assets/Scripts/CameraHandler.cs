using System;
using Unity.Mathematics;
using UnityEngine;

namespace SVell 
{
	public class CameraHandler : MonoBehaviour
	{
		public static CameraHandler Instance;

		[SerializeField] private float lookSpeed = 0.1f;
		[SerializeField] private float followSpeed = 0.1f;
		[SerializeField] private float pivotSpeed = 0.03f;
		[SerializeField] private float minimumPivot = -35f;
		[SerializeField] private float maximumPivot = 35;
		[Space] 
		[SerializeField] private float cameraSphereRadius = 0.2f;
		[SerializeField] private float cameraCollisionOffset = 0.2f;
		[SerializeField] private float minimumCollisionOffset = 0.2f;
		
		[SerializeField] private Transform targetTransform;
		[SerializeField] private Transform cameraTransform;
		[SerializeField] private Transform cameraPivotTransform;

		private Transform myTransform;
		private Vector3 cameraTransformPosition;
		private LayerMask ignoreLayers;
		private Vector3 cameraFollowVelocity = Vector3.zero;

		private float targetPosition;
		private float defaultPosition;
		private float lookAngle;
		private float pivotAngle;

		private void Awake()
		{
			Instance = this;
			
			myTransform = transform;
			defaultPosition = cameraTransform.localPosition.z;
			ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
		}

		public void FollowTarget(float delta)
		{
			Vector3 targetPos = Vector3.SmoothDamp(myTransform.position, targetTransform.position,
				ref cameraFollowVelocity, delta / followSpeed);
			myTransform.position = targetPos;
			
			HandleCameraCollisions(delta);
		}

		public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
		{
			lookAngle += mouseXInput * lookSpeed / delta;
			pivotAngle -= mouseYInput * pivotSpeed / delta;
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
			Vector3 dir = cameraTransform.position - cameraPivotTransform.position;
			dir.Normalize();

			if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, dir, out var hit,
				    Mathf.Abs(targetPosition), ignoreLayers))
			{
				float distance = Vector3.Distance(cameraPivotTransform.position, hit.point);
				targetPosition = -(distance - cameraCollisionOffset);
			}

			if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
			{
				targetPosition = -minimumCollisionOffset;
			}

			cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
			cameraTransform.localPosition = cameraTransformPosition;
		}
	}
}
