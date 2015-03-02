using UnityEngine;
using System.Collections;

public class PickupItem : MonoBehaviour 
{
	void OnTriggerEnter2D(Collider2D col)
	{ 
		if (col.gameObject.tag == "Sapphire") 
		{ Destroy (col.gameObject); } 
	} 
}