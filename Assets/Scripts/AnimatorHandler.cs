using UnityEngine;

namespace SVell 
{
	public class AnimatorHandler : MonoBehaviour
	{
		[SerializeField] private bool canRotate;
		[SerializeField] private InputHandler inputHandler;
		[SerializeField] private PlayerLocomotion playerLocomotion;

		private Animator animator;
		
		private int vertical;
		private int horizontal;

		public Animator Animator => animator;
		public bool CanRotate => canRotate;

		public void Initialize()
		{
			animator = GetComponent<Animator>();
			inputHandler = GetComponentInParent<InputHandler>();
			playerLocomotion = GetComponentInParent<PlayerLocomotion>();
			vertical = Animator.StringToHash("Vertical");
			horizontal = Animator.StringToHash("Horizontal");
		}

		public void UpdateAnimatorValues(float vertiacalMovement, float horizontalMovement, bool isSprinting)
		{
			#region Vertical
			float v = 0;

			if (vertiacalMovement > 0 && vertiacalMovement <= 0.55f)
			{
				v = 0.5f;
			}
			else if (vertiacalMovement > 0.55f)
			{
				v = 1;
			}
			else if (vertiacalMovement < 0 && vertiacalMovement > -0.55f)
			{
				v = -0.5f;
			}
			else if (vertiacalMovement < -0.55f)
			{
				v = -1;
			}
			else
			{
				v = 0;
			}
			#endregion
			
			#region Horizontal
			float h = 0;

			if (horizontalMovement > 0 && horizontalMovement <= 0.55f)
			{
				h = 0.5f;
			}
			else if (horizontalMovement > 0.55f)
			{
				h = 1;
			}
			else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
			{
				h = -0.5f;
			}
			else if (horizontalMovement < -0.55f)
			{
				h = -1;
			}
			else
			{
				h = 0;
			}
			#endregion

			if (isSprinting)
			{
				v = 2;
				h = horizontalMovement;
			}
			
			animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
			animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
		}

		public void PlayTargetAnimation(string targetAnim, bool isInteracting)
		{
			animator.applyRootMotion = isInteracting;
			animator.SetBool("IsInteracting", isInteracting);
			animator.CrossFade(targetAnim, 0.2f);
		}

		public void StartRotation()
		{
			canRotate = true;
		}

		public void StopRotation()
		{
			canRotate = false;
		}

		private void OnAnimatorMove()
		{
			if(!inputHandler.IsInteracting) return;

			float delta = Time.deltaTime;
			playerLocomotion.Rigidbody.drag = 0;
			Vector3 deltaPos = animator.deltaPosition;
			deltaPos.y = 0;
			Vector3 velocity = deltaPos / delta;

			playerLocomotion.Rigidbody.velocity = velocity;
		}
	}
}
