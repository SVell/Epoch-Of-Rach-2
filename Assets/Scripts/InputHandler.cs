using UnityEngine;
using UnityEngine.InputSystem;

namespace SVell 
{
	public class InputHandler : MonoBehaviour
	{
		[SerializeField] private float horizontal;
		[SerializeField] private float vertical;
		[SerializeField] private float moveAmount;
		[SerializeField] private float mouseX;
		[SerializeField] private float mouseY;

		[SerializeField] private bool bInput;
		[SerializeField] private bool rollFlag;
		[SerializeField] private bool isInteracting;

		private PlayerControls inputActions;
		private CameraHandler cameraHandler;

		private Vector2 movementInput;
		private Vector2 cameraInput;

		public float Horizontal => horizontal;
		public float Vertical => vertical;
		public float MoveAmount => moveAmount;

		public bool RollFlag
		{
			get => rollFlag;
			set => rollFlag = value;
		}

		public bool IsInteracting
		{
			get => isInteracting;
			set => isInteracting = value;
		}

		

		public void OnEnable()
		{
			inputActions ??= new PlayerControls();
			inputActions.PlayerMovement.Movement.performed +=
				inputActions => movementInput = inputActions.ReadValue<Vector2>();
			inputActions.PlayerMovement.Camera.performed +=
				inputActions => cameraInput = inputActions.ReadValue<Vector2>();
			
			inputActions.Enable();
		}

		private void OnDisable()
		{
			inputActions.Disable();
		}
		
		private void Start()
		{
			cameraHandler = CameraHandler.Instance;
		}

		private void FixedUpdate()
		{
			float delta = Time.fixedDeltaTime;

			if (cameraHandler != null)
			{
				cameraHandler.FollowTarget(delta);
				cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
			}
		}

		public void TickInput(float delta)
		{
			MoveInput(delta);
			HandleRollingInput(delta);
		}

		private void MoveInput(float delta)
		{
			horizontal = movementInput.x;
			vertical = movementInput.y;
			moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
			
			mouseX = cameraInput.x;
			mouseY = cameraInput.y;
		}

		private void HandleRollingInput(float delta)
		{
			bInput = inputActions.PlayerActions.Roll.phase == InputActionPhase.Performed;
			
			if (bInput)
			{
				rollFlag = true;
			}
		}
	}
}
