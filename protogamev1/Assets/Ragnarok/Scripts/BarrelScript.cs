using UnityEngine;
using System.Collections;

public class BarrelScript : MonoBehaviour {
	public int maxHealth = 3;
	private int damage = 0;
	private Animator banimator;

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
		banimator = GetComponent<Animator>();
		//TakeDamage (1);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (damage == 1) 
		{
			banimator.Play (Animator.StringToHash ("BarrelHit1"));
		}
		if (damage == 2) 
		{
			banimator.Play (Animator.StringToHash ("BarrelHit2"));
		}
	}
}
