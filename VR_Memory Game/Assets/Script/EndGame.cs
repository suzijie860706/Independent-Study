using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {
	public MemoryGame_Control memoryGame_Control;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void hitByRaycast()
	{
		if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Mouse0) || memoryGame_Control._VR)
		{
			Application.Quit ();
		}
	}
	private void OnTriggerStay(Collider collider){
		if (memoryGame_Control._VR == true) {
			VR_Control.Control_right.SendMessage ("Check", gameObject, SendMessageOptions.DontRequireReceiver);
		}
	}
}
