using UnityEngine;

namespace SVell 
{
	public class PlayerManager : MonoBehaviour
	{
		private InputHandler inputHandler;
		private CameraHandler cameraHandler;
		private PlayerLocomotion playerLocomotion;
		private Animator animator; 
		
		// Flags
		public bool IsInteracting { get; set; }
		public bool IsSprinting { get; set; }


		private void Awake()
		{
			inputHandler = GetComponent<InputHandler>();
			animator = GetComponentInChildren<Animator>();
			playerLocomotion = GetComponent<PlayerLocomotion>();
		}

		private void Start()
		{
			cameraHandler = CameraHandler.Instance;		
		}

		private void Update()
		{
			float delta = Time.deltaTime;
			
			IsInteracting = animator.GetBool("IsInteracting");
				
			inputHandler.TickInput(delta);
			playerLocomotion.HandleMovement(delta);
			playerLocomotion.HandleRollingAndSprint(delta);
		}

		private void FixedUpdate()
		{
			float delta = Time.fixedDeltaTime;

			if (cameraHandler != null)
			{
				cameraHandler.FollowTarget(delta);
				cameraHandler.HandleCameraRotation(delta, inputHandler.MouseX, inputHandler.MouseY);
			}
		}

		private void LateUpdate()
		{
			inputHandler.RollFlag = false;
			inputHandler.SprintFlag = false;
			
			IsSprinting = inputHandler.BInput;
		}
	}
}
