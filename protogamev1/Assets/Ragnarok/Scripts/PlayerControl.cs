using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	
	public static GameObject WalkedOverObject;
	// movement config
	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;
	public bool isAttacking = false;
	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;
	
	private CharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
	
	void Awake()
	{
		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController2D>();
		
		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
	}
	
	
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
		Debug.Log( "onTriggerEnterEvent: " + col.gameObject.tag );
		if (col.gameObject.tag == "Sapphire") {
			Destroy (col.gameObject);
			//this.GetComponent<healthScript>().health -= 1;
		} else if (col.gameObject.tag == "Coin") {
			Destroy (col.gameObject);
		} else if (col.gameObject.tag == "Diamond") {
			Destroy (col.gameObject);
		} else if (col.gameObject.tag == "GodRune") {
			Destroy (col.gameObject);
		} else if (col.gameObject.tag == "StrRune") {
			Destroy (col.gameObject);
		} else if (col.gameObject.tag == "GrowthRune") {
			Destroy (col.gameObject);
		} else if (col.gameObject.tag == "JoyRune") {
			Destroy (col.gameObject);
		} else if (col.gameObject.tag == "PowerRune") {
			Destroy (col.gameObject);
		} else if (col.gameObject.tag == "Barrell") {
			Debug.Log ("Walked Into Barrel");

		}

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
		
		if( _controller.isGrounded )
			_velocity.y = 0;
		
		if (Input.GetKey (KeyCode.RightArrow)) {
			normalizedHorizontalSpeed = 1;
			if (transform.localScale.x < 0f)
				transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			
			if (_controller.isGrounded)
				_animator.Play (Animator.StringToHash ("Run"));
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			normalizedHorizontalSpeed = -1;
			if (transform.localScale.x > 0f)
				transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			
			if (_controller.isGrounded)
				_animator.Play (Animator.StringToHash ("Run"));
		} else if (Input.GetKey (KeyCode.Z)) {
			normalizedHorizontalSpeed = 0;
			if (_controller.isGrounded)
			{
				_animator.Play( Animator.StringToHash( "Attack" ) );
				isAttacking = true;

			}
		}
		else
		{
			normalizedHorizontalSpeed = 0;
			
			if( _controller.isGrounded )
				_animator.Play( Animator.StringToHash( "Idle" ) );
		}
		if (Input.GetKeyUp (KeyCode.Z)) {
			isAttacking = false;
			Debug.Log( "Stopped Attacking");
		}
		
		// we can only jump whilst grounded
		if( _controller.isGrounded && Input.GetKeyDown( KeyCode.Space ) )
		{
			_velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
			_animator.Play( Animator.StringToHash( "Jump" ) );
		}


		// apply horizontal speed smoothing it
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );
		
		// apply gravity before moving
		_velocity.y += gravity * Time.deltaTime;
		_controller.move( _velocity * Time.deltaTime );
	}
	
}