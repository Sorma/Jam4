using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorDetector : MonoBehaviour {
	public bool isWall = false;

	// Use this for initialization

	void Update()
	{
		
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Wall")){
			isWall = false;
		} 

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Wall")){
			Debug.Log("is wall");
			isWall = true;
		}
		else{
			isWall = false;
		}
	}




	public bool checkIsWall(){
		return isWall;
	}
	public void resetIsFloor(){
		isWall = false;
	}
}
