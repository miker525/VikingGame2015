using UnityEngine;
using System.Collections;

public class CrateScript : MonoBehaviour {

	public int maxHealth = 6;
	private int damage = 0;
	private Animator canimator;
	
	public void TakeDamage(int damageamt)
	{
		if (damage < maxHealth) 
		{
			damage+=damageamt;
			Debug.Log ("Damage Taken");
		}
		
	}
	
	public int CheckDamage()
	{
		return damage;
	}
	
	
	// Use this for initialization
	void Awake () 
	{
		canimator = GetComponent<Animator>();
		//TakeDamage (3);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (damage >= maxHealth/2) 
		{
			canimator.Play (Animator.StringToHash ("cratedamaged"));
		}
	}
}
