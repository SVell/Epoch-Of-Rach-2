using UnityEngine;

namespace SVell 
{
	public class AnimatorHandler : MonoBehaviour
	{
		[SerializeField] private bool canRotate;

		private Animator animator;
		
		private int vertical;
		private int horizontal;

		public bool CanRotate => canRotate;

		public void Initialize()
		{
			animator = GetComponent<Animator>();
			vertical = Animator.StringToHash("Vertical");
			horizontal = Animator.StringToHash("Horizontal");
		}

		public void UpdateAnimatorValues(float vertiacalMovement, float horizontalMovement)
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
			
			animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
			animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
		}

		public void StartRotation()
		{
			canRotate = true;
		}

		public void StopRotation()
		{
			canRotate = false;
		}
	}
}
