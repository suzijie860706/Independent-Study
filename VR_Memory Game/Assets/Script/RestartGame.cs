using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class RestartGame : MonoBehaviour {
	public MemoryGame_Control memoryGame_Control;
	public GameObject StartBm; //開始按鈕物件
  
//	private Text timeText; //顯示時間文字
//	private Text scoreText;//顯示分數文字


	// Use this for initialization
	void Start () {
		GameObject.Find ("StartButton");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void hitByRaycast()
    {
		if ((Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Mouse0) || memoryGame_Control._VR) 
			&& MemoryGame_Control.gameSwitch)
       {
			SceneManager.LoadScene ("First");
			MemoryGame_Control.gameSwitch = false;
			cardAnimation.CountCard = 0;
       }
    }
	private void OnTriggerStay(Collider collider){
		if (memoryGame_Control._VR == true) {
			VR_Control.Control_right.SendMessage ("Check", gameObject, SendMessageOptions.DontRequireReceiver);
		}
	}
}
