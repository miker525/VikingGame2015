using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneScript : MonoBehaviour 
{
	public int numOfScenes = 1;
	public string followingLevel = "demoLevel";
	private Animator animator;
	private float skipTimer = 2f;
	private float waitTimer = 4f;
	private int currentScene = 1;
	private GameObject text;

	// Use this for initialization
	void Awake () 
	{
		animator = GetComponent<Animator>();
		text = GameObject.Find ("InfoText");
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Space bar is used to progress to the next part of the scene.
		if (Input.GetKeyUp (KeyCode.Space)) 
		{
			currentScene++;
			string nextScene = "Img" + currentScene.ToString ();
			waitTimer = 4f;
			if (currentScene >= numOfScenes+1)
			{
				Application.LoadLevel (followingLevel);
			}
			else
			{
				animator.Play (Animator.StringToHash (nextScene));
			}
		}

		//Enter key used to skip cutscene.
		if (Input.GetKey (KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter)) 
		{
			waitTimer = 4f;
			if (skipTimer > 0)
			{
				skipTimer -= Time.deltaTime;
			}
			else if (skipTimer <= 0 )
			{
				Application.LoadLevel (followingLevel);
			}
		}
		if (Input.GetKeyUp (KeyCode.Return)|| Input.GetKey(KeyCode.KeypadEnter)) 
		{
			skipTimer = 3f;
		}

		//Show tips if sitting for too long
		if (waitTimer > 0) 
		{
			waitTimer -= Time.deltaTime;
			text.SetActive(false);
		} 
		else if (waitTimer <= 0) 
		{
			text.SetActive(true);
		}

	}
}
