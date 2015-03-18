using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	private float maxSqrDistance = 4; //4 meters 
	public float moveSpeed = 1f; //So you can tweak move speed in the editor or in enemy prefabs
	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;
	public GameObject target;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;
	private CharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
	private bool facingRight = true;
	private bool isDead = false;
	private double Health = 100.00;

	void Awake()
	{
		_animator = GetComponent<Animator> ();
		_controller = GetComponent<CharacterController2D> ();
		_controller.onControllerCollidedEvent += onControllerCollider;
	}

	void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;
	}

	void Flip()
	{
		// Switch the way the player is labelled as facing
		facingRight = !facingRight;
		// Multiply the player's x local scale by -1
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	void Update () {
		
		//Vector between the 2 entities
		Vector3 dir = target.transform.position - transform.position;
		
		float distance = dir.sqrMagnitude, //Length squared because a sqrt is expensive
		dot = Vector3.Dot(transform.forward, dir.normalized);
		
		if (distance < maxSqrDistance && distance > .1) 
		{       //Is the distance less than what we want?
			Debug.Log (distance);
			if (dir.normalized.x >= 1) 
			{ //Walk Right
				if (facingRight == false)
					Flip();
				if (_controller.isGrounded)
					_animator.Play (Animator.StringToHash ("Walk"));
			} 
			else if (dir.normalized.x <= 1) 
			{ //Walk Left
				if (facingRight == true)
					Flip();

				if (_controller.isGrounded)
					_animator.Play (Animator.StringToHash ("Walk"));
			}
			//Move in the direction, so towards the player
			transform.position += dir.normalized * moveSpeed * Time.deltaTime;
		} else {

			if (_controller.isGrounded)
				_animator.Play (Animator.StringToHash ("Idle"));
		}
	}
}
