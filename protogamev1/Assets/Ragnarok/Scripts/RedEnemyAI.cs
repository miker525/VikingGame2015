using UnityEngine;
using System.Collections;

public class RedEnemyAI : MonoBehaviour {
	public GameObject target;
	public double health = 100.00;

	[HideInInspector]
	private float gravity = -25f;
	private float runSpeed = 1f;
	private float groundDamping = 20f; // how fast do we change direction? higher means faster
	private float inAirDamping = 5f;
	private float jumpHeight = 3f;
	private float maxSqrDistance = 4;
	private float normalizedHorizontalSpeed = 0;
	private bool isDead = false;
	private bool isAttacking = false;
	private bool isJumping = false;
	private bool isRunning = false;
	private bool isIdle = true;
	private bool isHurt = false;
	private bool isFacingRight = true;
	private Animator animatorz;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 velocity;
	private CharacterController2D _controller;

	void Flip()
	{
		isFacingRight = !isFacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void TakeDamage(double dmgamt)
	{
		health -= dmgamt;
		isHurt = true;
	}

	// Use this for initialization
	void Awake()
	{
		animatorz = GetComponent<Animator> ();
		_controller = GetComponent<CharacterController2D> ();
		_controller.onControllerCollidedEvent += onControllerCollider;
	}
	void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 dir = target.transform.position - transform.position;
		float distance = dir.sqrMagnitude, //Length squared because a sqrt is expensive
		dot = Vector3.Dot(transform.forward, dir.normalized);
		velocity = _controller.velocity;
		if( _controller.isGrounded )
			velocity.y = 0;
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		//velocity.x = Mathf.Lerp( velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );
		if (distance < maxSqrDistance && distance > .4) //.1
		{
			Debug.Log(dir.normalized.x);
			int anim = 0;
			if (dir.normalized.x > 0) 
			{ 
				//Run Right
				if (isFacingRight == false)
					Flip();
			}
			else if (dir.normalized.x < 0)
			{
				//Run Left
				if (isFacingRight == true)
					Flip();
			}
			velocity.x = Mathf.Lerp( velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );
			animatorz.Play (Animator.StringToHash ("Walk"));
			transform.Translate(dir.normalized.x * runSpeed * Time.deltaTime, 0, 0);
		} 
		else 
		{
			animatorz.Play (Animator.StringToHash ("Idle"));
		}




	}
}
