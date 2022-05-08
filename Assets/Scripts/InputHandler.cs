using UnityEngine;
using UnityEngine.InputSystem;

namespace SVell 
{
	public class InputHandler : MonoBehaviour
	{
		private PlayerControls inputActions;

		private Vector2 movementInput;
		private Vector2 cameraInput;

		private float horizontal;
		private float vertical;
		private float moveAmount;
		private float mouseX;
		private float mouseY;

		private float rollInputTimer;
		
		public bool SprintFlag { get; set; }
		public bool BInput { get; set; }
		public bool RollFlag { get; set; }

		public float Horizontal => horizontal;
		public float Vertical => vertical;
		public float MoveAmount => moveAmount;
		public float MouseX => mouseX;
		public float MouseY => mouseY;


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
			BInput = inputActions.PlayerActions.Roll.phase == InputActionPhase.Performed;
			
			if (BInput)
			{
				rollInputTimer += delta;
				SprintFlag = true;
			}
			else
			{
				if (rollInputTimer > 0 && rollInputTimer < 0.5f)
				{
					SprintFlag = false;
					RollFlag = true;
				}

				rollInputTimer = 0;
			}
		}
	}
}
