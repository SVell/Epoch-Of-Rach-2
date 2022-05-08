using UnityEngine;

namespace SVell 
{
	public class PlayerLocomotion : MonoBehaviour
	{
		[Header("Movement Stats")] 
		[SerializeField] private float movementSpeed = 5f;
		[SerializeField] private float sprintSpeed = 7f;
		[SerializeField] private float rotationSpeed = 10f;

		private PlayerManager playerManager;
		private InputHandler inputHandler;
		private AnimatorHandler animatorHandler;
		private new Rigidbody rigidbody;
		
		private GameObject normalCamera;
		private Transform camera;

		private Vector3 moveDirection;

		public Transform MyTransform => transform;
		public Rigidbody Rigidbody => rigidbody;

		private void Awake()
		{
			playerManager = GetComponent<PlayerManager>();
			rigidbody = GetComponent<Rigidbody>();
			inputHandler = GetComponent<InputHandler>();

			animatorHandler = GetComponentInChildren<AnimatorHandler>();
			animatorHandler.Initialize();
			
			camera = Camera.main.transform;
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

		public void HandleMovement(float delta)
		{
			if(inputHandler.RollFlag) return;
			
			moveDirection = camera.forward * inputHandler.Vertical;
			moveDirection += camera.right * inputHandler.Horizontal;
			moveDirection.Normalize();
			moveDirection.y = 0;

			float speed = movementSpeed;

			if (inputHandler.SprintFlag)
			{
				speed = sprintSpeed;
				playerManager.IsSprinting = true;
				moveDirection *= speed;
			}
			else
			{
				moveDirection *= speed;
			}

			Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
			rigidbody.velocity = projectedVelocity;

			animatorHandler.UpdateAnimatorValues(inputHandler.MoveAmount, 0, 
				playerManager.IsSprinting);

			if (animatorHandler.CanRotate)
			{
				HandleRotation(delta);
			}
		}

		public void HandleRollingAndSprint(float delta)
		{
			if (animatorHandler.Animator.GetBool("IsInteracting")) return;

			if (inputHandler.RollFlag)
			{
				moveDirection = camera.forward * inputHandler.Vertical;
				moveDirection += camera.right * inputHandler.Horizontal;

				if (inputHandler.MoveAmount > 0)
				{
					animatorHandler.PlayTargetAnimation("Rolling", true);
					moveDirection.y = 0;
					Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
					MyTransform.rotation = rollRotation;
				}
				else
				{
					animatorHandler.PlayTargetAnimation("Backstep", true);
				}
			}
		}
		
		#endregion
	}
}
