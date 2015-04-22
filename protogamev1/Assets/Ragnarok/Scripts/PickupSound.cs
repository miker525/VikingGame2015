using UnityEngine;
using System.Collections;

public class PickupSound : MonoBehaviour {

	// Use this for initialization
	void Awake () {
	
	}

	void onTriggerEnterEvent( Collider2D col )
	{
		audio.Play();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
