using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class LCD_Collision : MonoBehaviour {
	public MemoryGame_Control memoryGame_Control;
	public FirstPersonController _firstPersonController;
	//滑鼠
	MouseLook _lock = new MouseLook();	//鎖定鏡頭

	//切換拼圖場景
	void OnTriggerStay(Collider collider){
		if (memoryGame_Control._VR == true) {
			VR_Control.Control_right.transform.SendMessage ("Check", gameObject, SendMessageOptions.DontRequireReceiver);
		}
	}
	public void hitByRaycast()
	{
		_firstPersonController.enabled = false;
		_lock.SetCursorLock (false);//不鎖定滑鼠
		UnityEngine.SceneManagement.SceneManager.LoadScene ("GameScene0");
	}
}
