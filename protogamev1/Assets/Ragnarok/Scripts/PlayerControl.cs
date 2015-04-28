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
	public float AttackRadius = 0.3f;
	private bool isAttacking = false;
	private bool hasSword = false;
	private bool hasMace = false;
	private int health = 5;
	private int Weapon = 1;
	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;
	private CharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
	private bool isDead = false;
	private bool isHurt = false;
	private bool isFlashing = false;
	private float swingtime = .28f;
	private float dmgtime = 2.5f;
	private AudioSource[] sounds;


	void Awake()
	{
		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController2D>();
		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
		sounds = GetComponents<AudioSource> ();
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
			sounds [2].Play ();
			Destroy (col.gameObject);
			//this.GetComponent<healthScript>().health -= 1;
		} else if (col.gameObject.tag == "Coin") {
			sounds [1].Play ();
			Destroy (col.gameObject);
		} else if (col.gameObject.tag == "Diamond") {
			sounds [2].Play ();
			Destroy (col.gameObject);
		} else if (col.gameObject.tag == "Emerald") {
			sounds [2].Play ();
			Destroy (col.gameObject);
		} else if (col.gameObject.tag == "GodRune") {
			sounds [3].Play ();
			Destroy (col.gameObject);
			col.GetComponent<RuneEffect> ().ApplyRune ();
		} else if (col.gameObject.tag == "StrRune") {
			sounds [3].Play ();
			Destroy (col.gameObject);
		} else if (col.gameObject.tag == "GrowthRune") {
			sounds [3].Play ();
			Destroy (col.gameObject);
		} else if (col.gameObject.tag == "JoyRune") {
			sounds [3].Play ();
			Destroy (col.gameObject);
		} else if (col.gameObject.tag == "PowerRune") {
			sounds [3].Play ();
			Destroy (col.gameObject);
		} else if (col.gameObject.tag == "Respawn") {
			health = 0;
		} else if (col.gameObject.tag == "Sword") {
			if (!hasSword)
			{
				hasSword = true;
			}
			Weapon = 2;
			Destroy (col.gameObject);
		}

	}
	
	
	void onTriggerExitEvent( Collider2D col )
	{
		//Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}
	
	#endregion
	
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
	public void Kill()
	{
		isDead = true;
	}
	public bool checkHurt()
	{
		return isHurt;
	}
	
	public bool checkDead()
	{
		return isDead;
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




	// the Update loop contains a very simple example of moving the character around and controlling the animation
	void Update()
	{
		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
		
		if( _controller.isGrounded )
			_velocity.y = 0;

		if (isHurt) 
		{
			isAttacking = false;
			if (!isFlashing) {
				StartCoroutine (Flash ());
			}
			if (dmgtime > 0) 
			{
				dmgtime -= Time.deltaTime;
			}
			if (dmgtime < 0) 
			{
				isHurt = false;
			}
		} 
		else 
		{
			isHurt = false;
			isFlashing = false;
		}



		
		if (Input.GetKey (KeyCode.RightArrow)) 
		{
			normalizedHorizontalSpeed = 1;
			if (transform.localScale.x < 0f)
				transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			
			if (_controller.isGrounded)
				_animator.Play (Animator.StringToHash ("Run"));
		} 
		else if (Input.GetKey (KeyCode.LeftArrow)) 
		{
			normalizedHorizontalSpeed = -1;
			if (transform.localScale.x > 0f)
				transform.localScale = new Vector3 (-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			
			if (_controller.isGrounded)
				_animator.Play (Animator.StringToHash ("Run"));
		} 
		else if (Input.GetKey (KeyCode.Z)) 
		{
			normalizedHorizontalSpeed = 0;
			if (_controller.isGrounded)
			{
				if (hasSword && Weapon == 2)
				{
					_animator.Play( Animator.StringToHash( "SwordAttack" ) );
				}
				else if (hasMace && Weapon == 3)
				{
					_animator.Play( Animator.StringToHash( "MaceAttack" ) );
				}
				else
				{
					_animator.Play( Animator.StringToHash( "Attack" ) );
				}
				isAttacking = true;
			}
			int environmentLayerMask = 1 << LayerMask.NameToLayer("Enivornment");
			int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
			//Collider[] overlappedThings = Physics2D.OverlapCircleAll(transform.position.x, transform.position.y, 2f, environmentLayerMask);

			if (swingtime > 0) 
			{
				swingtime -= Time.deltaTime;
			} 
			else if (swingtime <= 0) 
			{
				/* Checking For Objects */
				Collider2D[] overlappedThings = Physics2D.OverlapCircleAll(transform.position, AttackRadius, environmentLayerMask);
				for (int i=0;i<overlappedThings.Length;i++)
				{
					GameObject e = overlappedThings[i].gameObject;

					Debug.Log (e.tag);
					if (e.tag == "Barrell")
					{
						if (e.GetComponent<BarrelScript>().CheckDamage() >= e.GetComponent<BarrelScript>().maxHealth)
						{
							sounds[4].Play ();
							Destroy(e.gameObject);
						}
						else
						{
							sounds[5].Play ();
							e.GetComponent<BarrelScript>().TakeDamage(1);
						}
					}
					else if (e.tag == "Crate")
					{
						if (e.GetComponent<CrateScript>().CheckDamage() >= e.GetComponent<CrateScript>().maxHealth)
						{
							sounds[4].Play ();
							Destroy(e.gameObject);
						}
						else
						{
							sounds[5].Play ();
							e.GetComponent<CrateScript>().TakeDamage(1);
						}

					}
				}

				/*Checking For Enemies */
				Collider2D[] overlappedEnemies = Physics2D.OverlapCircleAll(transform.position, AttackRadius, enemyLayerMask);
				for (int i=0;i<overlappedEnemies.Length;i++)
				{
					GameObject e = overlappedEnemies[i].gameObject;
					//Debug.Log (e.tag);
					if (e.tag == "Enemy")
					{
						if (!e.GetComponent<RedEnemyAI>().checkDead() && !e.GetComponent<RedEnemyAI>().checkHurt())
						{
							sounds[5].Play ();
							e.GetComponent<RedEnemyAI>().TakeDamage (2);
						}
						else if (e.GetComponent<RedEnemyAI>().checkDead())
						{
							Destroy(e.gameObject);
						}
					}
				}

				swingtime = .28f;
			}
		
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			if (Weapon == 1 && hasSword)
			{
				Weapon = 2;
			}
			else if (Weapon == 2 && hasMace)
			{
				Weapon = 3;
			}
			else if (Weapon == 2 && !hasMace)
			{
				Weapon = 1;
			}
			else if (Weapon == 3)
			{
				Weapon = 1;
			}
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			if (Weapon == 1 && hasMace)
			{
				Weapon = 3;
			}
			else if (Weapon == 1 && !hasMace && hasSword)
			{
				Weapon = 2;
			}
			else if (Weapon == 2)
			{
				Weapon = 1;
			}
			else if (Weapon == 3)
			{
				Weapon = 2;
			}
		}
		else
		{
			normalizedHorizontalSpeed = 0;
			
			if( _controller.isGrounded )
				_animator.Play( Animator.StringToHash( "Idle" ) );

			isAttacking = false;
		}


		// we can only jump whilst grounded
		if( _controller.isGrounded && Input.GetKeyDown( KeyCode.Space ) )
		{
			_velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
			_animator.Play( Animator.StringToHash( "Jump" ) );
			sounds[0].Play();
			//audio.Play();
		}

		if (health <= 0) {
			isDead = true;
		}

		if (isDead) {
			Application.LoadLevel (Application.loadedLevel);
		}
		// apply horizontal speed smoothing it
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );
		
		// apply gravity before moving
		_velocity.y += gravity * Time.deltaTime;
		_controller.move( _velocity * Time.deltaTime );
	}
	
}