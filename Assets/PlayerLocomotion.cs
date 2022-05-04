using System;
using UnityEngine;

namespace SVell 
{
	public class PlayerLocomotion : MonoBehaviour
	{
		[Header("Stats")] 
		[SerializeField] private float movementSpeed = 5f;
		[SerializeField] private float rotationSpeed = 10f;
		
		private InputHandler inputHandler;
		private AnimatorHandler animatorHandler;
		private new Rigidbody rigidbody;
		
		private GameObject normalCamera;
		private Transform camera;

		private Vector3 moveDirection;

		private Transform MyTransform => transform;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
			inputHandler = GetComponent<InputHandler>();

			animatorHandler = GetComponentInChildren<AnimatorHandler>();
			animatorHandler.Initialize();
			
			camera = Camera.main.transform;
		}

		public void Update()
		{
			float delta = Time.deltaTime;
			
			inputHandler.TickInput(delta);

			moveDirection = camera.forward * inputHandler.Vertical;
			moveDirection += camera.right * inputHandler.Horizontal;
			moveDirection.Normalize();

			float speed = movementSpeed;
			moveDirection *= speed;

			Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
			rigidbody.velocity = projectedVelocity;

			animatorHandler.UpdateAnimatorValues(inputHandler.MoveAmount, 0);
			
			if (animatorHandler.CanRotate)
			{
				HandleRotation(delta);
			}
		}

		#region Movement

		private Vector3 normalVector;
		private Vector3 targetPosition;

		private void HandleRotation(float delta)
		{
			Vector3 targetDir = Vector3.zero;
			float moveOverride = inputHandler.MoveAmount;

			targetDir = camera.forward * inputHandler.Vertical;
			targetDir += camera.right * inputHandler.Horizontal;

			targetDir.Normalize();
			targetDir.y = 0;

			if (targetDir == Vector3.zero)
			{
				targetDir = MyTransform.forward;
			}

			float rs = rotationSpeed;
			
			Quaternion tr = Quaternion.LookRotation(targetDir);
			Quaternion targetRotation = Quaternion.Slerp(MyTransform.rotation, tr, rotationSpeed * delta);

			MyTransform.rotation = targetRotation;
		}

		#endregion
	}
}
