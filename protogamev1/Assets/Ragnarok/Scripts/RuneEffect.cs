using UnityEngine;
using System.Collections;

public class RuneEffect : MonoBehaviour 
{
	public Transform target;
	public float time = 30.0f;
	// Use this for initialization
	void Awake () 
	{

	}

	public void ApplyRune()
	{
		if (transform.tag == "GodRune") 
		{
			if (time > 0) 
			{
				time -= Time.deltaTime;

			} 
			else if (time <= 0) 
			{

			}
		} 
		else if (transform.tag == "StrRune") 
		{

		}
		else if (transform.tag == "GrowthRune") 
		{
			
		}
		else if (transform.tag == "JoyRune") 
		{
			
		}
		else if (transform.tag == "PowerRune") 
		{
			
		}
	}
}
