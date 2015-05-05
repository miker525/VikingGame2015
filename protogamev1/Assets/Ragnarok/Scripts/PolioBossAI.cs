using UnityEngine;
using System.Collections;

public class PolioBossAI : MonoBehaviour 
{
	public GameObject target;

	private CharacterController2D _controller;
	private Animator animatorz;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
	private float maxSqrDistance = 4;
	private float normalizedHorizontalSpeed = 0;
	private float gravity = -120f;
	private float groundDamping = 20f; // how fast do we change direction? higher means faster
	private float inAirDamping = 5f;
	private float jumpHeight = .75f;
	private Vector3 velocity;
	private bool isChasing = true;
	private bool isFacingRight = true;
	private const float increaseInterval = 3f; // how often do you want the speed to change
	private const float change = 1f; // how much do you want the speed to change
	private const float maxSpeed = 3f;
	private const float minSpeed = .5f;
	private float currentSpeed = 1f;
	float timer = increaseInterval;

	void Flip()
	{
		isFacingRight = !isFacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void Chase(Vector3 moveTo)
	{
		if (moveTo.normalized.x > 0) 
		{ 
			//Run Right
			if (isFacingRight == false)
				Flip();
		}
		else if (moveTo.normalized.x < 0)
		{
			//Run Left
			if (isFacingRight == true)
				Flip();
		}
		//animatorz.Play (Animator.StringToHash ("Walk"));
		transform.Translate(moveTo.normalized.x * currentSpeed * Time.deltaTime, 0, 0);
	}

	void Awake()
	{
		_controller = GetComponent<CharacterController2D>();
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
	}


	void onControllerCollider( RaycastHit2D hit )
	{
		if( hit.normal.y == 1f )
			return;

	}
	
	//WALK INTO EVENTS
	void onTriggerEnterEvent( Collider2D col )
	{
		
	}
	
	
	void onTriggerExitEvent( Collider2D col )
	{
		
	}


	// Update is called once per frame
	void Update () 
	{
		if (currentSpeed < maxSpeed) 
		{
			if (timer > 0)
			{
				timer -= Time.deltaTime;
			}
			else if (timer <=0)
			{
				currentSpeed += change;
			}
		}

		_velocity = _controller.velocity;
		Vector3 dir = target.transform.position - transform.position;
		float distance = dir.sqrMagnitude, //Length squared because a sqrt is expensive
		dot = Vector3.Dot(transform.forward, dir.normalized);
		if( _controller.isGrounded )
			_velocity.y = 0;

		if (isChasing) 
		{
			Chase (dir);

			int environmentLayerMask = 1 << LayerMask.NameToLayer("Environment");
			int playerLayerMask = 1 << LayerMask.NameToLayer("Player");
			Collider2D[] overlappedPlayers = Physics2D.OverlapCircleAll(transform.position, 0.03f, playerLayerMask);
			Collider2D[] overlappedEnv = Physics2D.OverlapCircleAll(transform.position, 0.03f, environmentLayerMask);
			for (int i=0; i<overlappedEnv.Length; i++) 
			{
				GameObject e = overlappedPlayers [i].gameObject;
				Destroy(e);
				if (currentSpeed > .5f)
				{
					currentSpeed -= .5f;
				}
			}
			for (int i=0; i<overlappedPlayers.Length; i++) 
			{
				GameObject e = overlappedPlayers [i].gameObject;
				e.GetComponent<PlayerControl>().Kill ();
				isChasing = false;
			}

		}

		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * currentSpeed, Time.deltaTime * smoothedMovementFactor );
		
		// apply gravity before moving
		_velocity.y += gravity * Time.deltaTime;
		_controller.move( _velocity * Time.deltaTime );

	}
}
