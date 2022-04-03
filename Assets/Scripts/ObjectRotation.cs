
using UnityEngine;
using System.Collections;

public class ObjectRotation : MonoBehaviour {

	public float planetSpeedRotation = 1.0f;
	

	// rotation on y axis from left to right 
	//Time.deltaTime = time btw 2 frames 
	// it makes the rotation smoother
	void LateUpdate () {
	
		transform.Rotate(-Vector3.up * Time.deltaTime * planetSpeedRotation);
	}
}
