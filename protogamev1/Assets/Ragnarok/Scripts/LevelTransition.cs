using UnityEngine;
using System.Collections;

public class LevelTransition : MonoBehaviour 
{
	public string LevelName = "";
	// Use this for initialization
	
	public string getNextLevel()
	{
		return LevelName;
	}
}
