/*Response script that fires Unity's event system*/

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class QTE_Response_UnityEvent : MonoBehaviour
{



	public UnityEvent ActiveEvent = new UnityEvent ();
	public UnityEvent Button1Event = new UnityEvent ();
	public UnityEvent Button2Event = new UnityEvent ();
	public UnityEvent Button3Event = new UnityEvent ();
	public UnityEvent Button4Event = new UnityEvent ();
	public UnityEvent ButtonWrongEvent = new UnityEvent ();
	public UnityEvent ButtonFailEvent = new UnityEvent ();

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		
		if (QTE_main.Singleton.TriggeringObject == this.gameObject) {		
			
			//while the QTE is happening
			if (QTE_main.Singleton.QTEactive) {
				ActiveEvent.Invoke ();

			}

			if (QTE_main.Singleton.QTECompleted && !QTE_main.Singleton.QTEactive) {
			
				//if the QTE completed, and he succedded with option 1
				if (QTE_main.Singleton.succeeded) {

					Button1Event.Invoke ();

				}
			
				//if the QTE completed, and he succedded with option 2 (Dual QTE only)
				if (QTE_main.Singleton.succeeded2) {

					Button2Event.Invoke ();

				}
			
				//if the QTE completed, and he succedded with option 3 (Tri QTE only)
				if (QTE_main.Singleton.succeeded3) {

					Button3Event.Invoke ();

				}
			
				//if the QTE completed, and he succedded with option 4 (Quad QTE only)
				if (QTE_main.Singleton.succeeded4) {

					Button4Event.Invoke ();

				}
			
				//if the QTE completed, and he failed

				if (QTE_main.Singleton.QTE_Failed_WrongButton) {
					//If failed due to pressing the wrong button

					ButtonWrongEvent.Invoke ();

				}
				if (QTE_main.Singleton.QTE_Failed_timer) {
					//if failed due to the timer finishing.
					ButtonFailEvent.Invoke ();

				}

			}
		}
	
	}
}
