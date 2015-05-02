using UnityEngine;
using System.Collections;

public class HUDHealthScript : MonoBehaviour 
{
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
		if (player.GetComponent<PlayerControl> ().checkHealth() == 5) {
			animator.Play (Animator.StringToHash ("5heart"));
		} else if (player.GetComponent<PlayerControl> ().checkHealth() == 4) {
			animator.Play (Animator.StringToHash ("4heart"));
		} else if (player.GetComponent<PlayerControl> ().checkHealth() == 3) {
			animator.Play (Animator.StringToHash ("3heart"));
		} else if (player.GetComponent<PlayerControl> ().checkHealth() == 2) {
			animator.Play (Animator.StringToHash ("2heart"));
		} else if (player.GetComponent<PlayerControl> ().checkHealth() == 1) {
			animator.Play (Animator.StringToHash ("1heart"));
		} else if (player.GetComponent<PlayerControl> ().checkHealth() == 0) {
			animator.Play (Animator.StringToHash ("0heart"));
		}
	}
}
