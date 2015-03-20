using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	// movement config
	public GameObject target;
	public double health = 100.00;

	[HideInInspector]
	private static GameObject WalkedOverObject;
	private float normalizedHorizontalSpeed = 0;
	private float gravity = -120f;
	private float runSpeed = 1f;
	private float groundDamping = 20f; // how fast do we change direction? higher means faster
	private float inAirDamping = 5f;
	private float jumpHeight = .75f;
	private CharacterController2D _controller;
	private Animator animatorz;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
	private float maxSqrDistance = 4;
	private bool isDead = false;
	private bool isJumping = false;
	private bool isRunning = false;
	private bool isIdle = true;
	private bool isHurt = false;
	private bool isAttacking = false;
	private bool isFacingRight = true;
	private Vector3 velocity;
	private Vector3 Origin;
	float originReturnTimer = 4.0f;

	void Awake()
	{
		animatorz = GetComponent<Animator> ();
		_controller = GetComponent<CharacterController2D>();
		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
		Origin = transform.position;
	}

	#region Custom Functions

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

	void returnToOrigin(Vector3 pointOfOrigin)
	{
		
		if (originReturnTimer > 0) 
		{
			originReturnTimer -= Time.deltaTime;
		} 
		else if (originReturnTimer <= 0) 
		{
			Vector3 dir = pointOfOrigin - transform.position;
			if (dir.normalized.x > 0) 
			{ 
				if (isFacingRight == false)
					Flip();
			}
			else if (dir.normalized.x < 0)
			{
				if (isFacingRight == true)
					Flip();
			}
			transform.Translate(dir.normalized.x * runSpeed * Time.deltaTime, 0, 0);
		}
	}

	#endregion

	#region Event Listeners
	
	void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;
		
		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}
	
	//WALK INTO EVENTS
	void onTriggerEnterEvent( Collider2D col )
	{

	}
	
	
	void onTriggerExitEvent( Collider2D col )
	{
		//Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}
	
	#endregion
	
	
	// the Update loop contains a very simple example of moving the character around and controlling the animation
	void Update()
	{
		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
		Vector3 dir = target.transform.position - transform.position;
		float distance = dir.sqrMagnitude, //Length squared because a sqrt is expensive
		dot = Vector3.Dot(transform.forward, dir.normalized);

		if( _controller.isGrounded )
			_velocity.y = 0;
		
		//var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
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
			animatorz.Play (Animator.StringToHash ("Walk"));
			transform.Translate(dir.normalized.x * runSpeed * Time.deltaTime, 0, 0);
			if (originReturnTimer != 4.0f)
			{
				originReturnTimer = 4.0f;
			}
		} 
		else 
		{
			animatorz.Play (Animator.StringToHash ("Idle"));
			if (transform.position != Origin)
			{animatorz.Play (Animator.StringToHash ("Walk"));returnToOrigin (Origin);}
			else {animatorz.Play (Animator.StringToHash ("Idle"));}
		}
		
		// apply horizontal speed smoothing it
		//var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );
		
		// apply gravity before moving
		_velocity.y += gravity * Time.deltaTime;
		_controller.move( _velocity * Time.deltaTime );
	}
	
}

