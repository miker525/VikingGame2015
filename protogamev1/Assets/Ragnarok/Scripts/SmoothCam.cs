using UnityEngine;
using System.Collections;

public class SmoothCam : MonoBehaviour {
	
	public float dampTime = 0.025f;
	public Transform target;
	private Vector3 velocity = Vector3.zero;
	
	// Update is called once per frame
	void Update () 
	{
		if (target)
		{
			Vector3 point = camera.WorldToViewportPoint(target.position);
			Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, point.y, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}
		
	}
}