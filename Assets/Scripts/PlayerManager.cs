using UnityEngine;

namespace SVell 
{
	public class PlayerManager : MonoBehaviour
	{
		private InputHandler inputHandler;
		private Animator animator;

		private void Awake()
		{
			inputHandler = GetComponent<InputHandler>();
			animator = GetComponentInChildren<Animator>();
		}

		private void Update()
		{
			inputHandler.IsInteracting = animator.GetBool("IsInteracting");
			inputHandler.RollFlag = false;
			inputHandler.SprintFlag = false;
		}
	}
}
