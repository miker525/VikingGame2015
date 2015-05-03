using UnityEngine;
using System.Collections;

public class HUDWeaponScript : MonoBehaviour {

	GameObject player;
	private Animator animator;
	// Use this for initialization
	void Start () 
	{
		animator = GetComponent<Animator>();
		player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Check Player's health and reflect in HUD
		if (player.GetComponent<PlayerControl> ().checkWeapon() == 1) {
			animator.Play (Animator.StringToHash ("WepHudAxe"));
			Debug.Log ("HUD Should Show Axe");
		} else if (player.GetComponent<PlayerControl> ().checkWeapon() == 2) {
			animator.Play (Animator.StringToHash ("WepHudSword"));
			Debug.Log ("HUD Should Show Sword");
		} else if (player.GetComponent<PlayerControl> ().checkHealth() == 3) {
			animator.Play (Animator.StringToHash ("WepHudMace"));
		} 
	}
}
