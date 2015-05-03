using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDScoreScript : MonoBehaviour {
	GameObject player;
	Text instruction;
	// Use this for initialization
	void Start () 
	{
		player = GameObject.Find ("Player");
		instruction = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Check Player's health and reflect in HU
		int curScore = player.GetComponent<PlayerControl> ().checkScore ();
		instruction.text = curScore.ToString ();
	}
}
