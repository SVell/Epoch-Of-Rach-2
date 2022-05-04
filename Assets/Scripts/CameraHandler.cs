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
		
		[SerializeField] private Transform targetTransform;
		[SerializeField] private Transform cameraTransform;
		[SerializeField] private Transform cameraPivotTransform;

		private Transform myTransform;
		private Vector3 cameraTransformPosition;
		private LayerMask ignoreLayers;

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
			Vector3 targetPos = Vector3.Lerp(myTransform.position, targetTransform.position, delta / followSpeed);
			myTransform.position = targetPos;
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
	}
}
