using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_MenuScript : MonoBehaviour {
	public MemoryGame_Control memoryGame_Control;

	string MenuString;

	void Start () {
		//_menu.GetComponent<Canvas> ().renderMode = RenderMode.WorldSpace;
	}

	void Update () {
		
	}

	private void OnTriggerStay(Collider collider){
		if (memoryGame_Control._VR == true) {
			MenuString = gameObject.name;
			VR_Control.Control_right.SendMessage ("VR_Menu", gameObject.name, SendMessageOptions.DontRequireReceiver);

		}
	}
}
