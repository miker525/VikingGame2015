using UnityEngine;
using System.Collections;

public class CrateScript : MonoBehaviour {

	public int maxHealth = 6;
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
		TakeDamage (3);
		Debug.Log ("Crate damage increased");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (damage == 3) 
		{
			//Sprite newSprite = Resources.Load("physical_1", typeof(Sprite)) as Sprite;
			Sprite newSprite = Resources.Load<Sprite>("physical_1");
			GetComponent<SpriteRenderer>().sprite = newSprite;
		}
	}
}
