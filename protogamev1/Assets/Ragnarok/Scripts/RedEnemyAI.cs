using UnityEngine;
using System.Collections;

public class RedEnemyAI : MonoBehaviour {
	// movement config
	public GameObject target;
	public double health = 100.00;
	public int Weapon = 1;
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
	private float originReturnTimer = 3.5f;
	private float hurtTimer = .40f;
	private float dmgtime = 2.5f;
	private bool isFlashing = false;

	void Awake()
	{
		animatorz = GetComponent<Animator> ();
		_controller = GetComponent<CharacterController2D>();
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
	
	public void TakeDamage(double dmgamt)
	{
		if (health - dmgamt <= 0) 
		{
			isDead = true;
		} 
		else 
		{
			health -= dmgamt;
			isHurt = true;
		}
	}

	public bool checkHurt()
	{
		return isHurt;
	}

	public bool checkDead()
	{
		return isDead;
	}
	
	void returnToOrigin(Vector3 pointOfOrigin)
	{
		//animatorz.StopPlayback();
		if (originReturnTimer > 0) 
		{
			animatorz.Play (Animator.StringToHash ("Idle"));
			originReturnTimer -= Time.deltaTime;
		} 
		else if (originReturnTimer <= 0) 
		{
			Vector3 dir = pointOfOrigin - transform.position;
			MoveTo (dir);
			//animatorz.Play (Animator.StringToHash ("Idle"));
		}
	}

	void MoveTo(Vector3 moveTo)
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
		animatorz.Play (Animator.StringToHash ("Walk"));
		transform.Translate(moveTo.normalized.x * runSpeed * Time.deltaTime, 0, 0);
	}

	void Retreat(Vector3 vec)
	{
		MoveTo (-vec);
	}

	IEnumerator Flash()
	{
		isFlashing = true;
		for(var n = 0; n < 10; n++)
		{
			renderer.enabled = true;
			yield return new WaitForSeconds(0.1f);
			renderer.enabled = false;
			yield return new WaitForSeconds(0.1f);
		}
		renderer.enabled = true;
	}
	
	#endregion
	
	#region Event Listeners
	
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

		/*if (isDead) 
		{
			if(gameObject.tag == "Enemy")
			{
				Destroy(this.gameObject);
			}
		}*/
		
		if (isHurt) 
		{
			isAttacking = false;
			if (!isFlashing)
			{
				StartCoroutine(Flash ());
			}
			if (dmgtime > 0)
			{
				Debug.Log ("POP");
				Retreat(dir);
				dmgtime -= Time.deltaTime;
			}
			if (dmgtime < 0)
			{
				isHurt = false;
				Debug.Log ("SAFE");
			}
		}

		if (isAttacking) 
		{
			if (Weapon == 1)
			{
				animatorz.Play(Animator.StringToHash ("AxeAttack"));
			}
			else if (Weapon == 2)
			{
				animatorz.Play (Animator.StringToHash ("SwordAttack"));
			}
		}

		//var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		if (distance < maxSqrDistance && distance > .18 && !isHurt) {
			MoveTo (dir);
			if (originReturnTimer != 3.5f) {
				originReturnTimer = 3.5f;
			}
		} else if (distance <= .20 && !isHurt) {
			isAttacking = true;
		}
		else 
		{
			if (transform.position.x <= Origin.x-.02 || transform.position.x >= Origin.x+.02)
			{

				returnToOrigin (Origin);
			}
			else 
			{
				animatorz.Play (Animator.StringToHash ("Idle"));
			}
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

