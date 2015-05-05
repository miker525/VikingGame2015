using UnityEngine;
using System.Collections;

public class CivScript : MonoBehaviour 
{
	public GameObject target; //= GameObject.Find ("Player");
	public int health = 2;
	private float normalizedHorizontalSpeed = 0;
	private float gravity = -120f;
	private float runSpeed = 2f;
	private float groundDamping = 20f;
	private float inAirDamping = 5f;
	private float jumpHeight = .75f;
	private CharacterController2D _controller;
	private Animator animatorz;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
	private float maxSqrDistance = 5;
	private bool isDead = false;
	private bool isRunning = false;
	private bool isIdle = true;
	private bool isHurt = false;
	private bool isFacingRight = true;
	private Vector3 velocity;
	private Vector3 Origin;
	private float hurtTimer = .40f;
	private bool isFlashing = false;
	private float dmgtime = 2.5f;
	
	// Use this for initialization
	void Awake()
	{
		animatorz = GetComponent<Animator> ();
		_controller = GetComponent<CharacterController2D>();
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
		//Origin = transform.position;
	}
	
	
	void Flip()
	{
		isFacingRight = !isFacingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	public void TakeDamage(int dmgamt)
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
		_velocity = _controller.velocity;
		Vector3 dir = target.transform.position - transform.position;
		float distance = dir.sqrMagnitude, //Length squared because a sqrt is expensive
		dot = Vector3.Dot(transform.forward, dir.normalized);
		if( _controller.isGrounded )
			_velocity.y = 0;
		
		if (distance < maxSqrDistance && distance > .18 && !isHurt) 
		{
			isRunning = true;
			//Retreat (dir);
			animatorz.Play (Animator.StringToHash ("Walk"));
		}
		
		if (isRunning) 
		{
			Retreat (dir);
		}
		
		if (isHurt) 
		{
			if (!isFlashing) 
			{
				StartCoroutine (Flash ());
			}
			if (dmgtime > 0) 
			{
				dmgtime -= Time.deltaTime;
				runSpeed = 3f;
			}
			if (dmgtime < 0) 
			{
				isHurt = false;
				dmgtime =  2.5f;
			}
		}
		
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );
		
		// apply gravity before moving
		_velocity.y += gravity * Time.deltaTime;
		_controller.move( _velocity * Time.deltaTime );
	}
}
