using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEPanelSwitch : MonoBehaviour {

	public GameObject QTEPanel;
	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player")){
			QTEPanel.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
