using UnityEngine;
using System.Collections;

public class Level_01_Background_Music : MonoBehaviour 
{
	AudioSource fxSound;
	// Use this for initialization
	void Start () 
	{
		fxSound = GetComponent<AudioSource> ();
		fxSound.Play ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!fxSound.isPlaying) 
		{
			fxSound.Play ();
		}
	}
}
