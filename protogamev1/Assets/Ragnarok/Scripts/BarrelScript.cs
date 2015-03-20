using UnityEngine;
using System.Collections;

public class BarrelScript : MonoBehaviour {
	public int maxHealth = 3;
	private int damage = 0;


	public void TakeDamage(int damageamt)
	{
		if (damage < maxHealth) 
		{
			damage++;
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
		//TakeDamage (1);
		//Debug.Log ("barrel damage increased");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (damage == 1) 
		{
			Sprite newSprite = Resources.Load("physical_3", typeof(Sprite)) as Sprite;
			GetComponent<SpriteRenderer>().sprite = newSprite;
		}
		if (damage == 2) 
		{
			Sprite newSprite = Resources.Load("physical_4", typeof(Sprite)) as Sprite;
			GetComponent<SpriteRenderer>().sprite = newSprite;
		}
	}
}
