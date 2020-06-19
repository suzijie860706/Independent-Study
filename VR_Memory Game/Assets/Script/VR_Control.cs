using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.SceneManagement;
public class VR_Control : MonoBehaviour {
	public static GameObject Control_right;
	public MemoryGame_Control memoryGame_Control;
	bool _button = false; // 限制次數用
	//VR手把初始值
	private SteamVR_TrackedObject _trackedObj;
	public SteamVR_Controller.Device device {
		get 
		{ return SteamVR_Controller.Input ((int)_trackedObj.index);}
		set
		{ SteamVR_Controller.Input ((int)_trackedObj.index);}
	}

	private bool biggerFirst = false; //拼圖用－－點選時，拼圖放大，才可以再放開時，拼圖縮小

	public GameObject VR_Escape_Children; //VR選單
	Scene scene; //場景
	void Start () {

		scene = SceneManager.GetActiveScene ();
		_trackedObj = GetComponent<SteamVR_TrackedObject> ();
		Control_right = gameObject;

	}

	void Update () {
		//手把震動回饋
//		if (SteamVR_Controller.Input (_rightHand).GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
//			SteamVR_Controller.Input(_rightHand).TriggerHapticPulse(1000);
//		}

		//選單
		if (scene.name == "First") {
			if (memoryGame_Control._VR == true) {
				if (device.GetPressDown (SteamVR_Controller.ButtonMask.ApplicationMenu)) {
					//偵測按鍵
					//Time.timeScale = 0.0001f;
					//VR_Escape_Children.transform.position = 
					VR_Escape_Children.SetActive (true);
				} else if (device.GetPressDown (SteamVR_Controller.ButtonMask.ApplicationMenu) &&
				           VR_Escape_Children.activeSelf == true) {
					VR_Escape_Children.SetActive (false);
				}
			}
		} 

//		else if (scene.name == "GameScene0") {
////			if (memoryGame_Control._VR == true) {
////				if (device.GetPressDown (SteamVR_Controller.ButtonMask.ApplicationMenu)) {
////				//	VR_Escape_Children.SetActive (true);
////				} else if (device.GetPressDown (SteamVR_Controller.ButtonMask.ApplicationMenu) &&
////					VR_Escape_Children.activeSelf == true) {
////					Time.timeScale = 1;
////					VR_Escape_Children.SetActive (false);
////				}
//			}
//		}
			
	}
	//常用雷射板機事件
	public void Check (GameObject obj){
		if (device.GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x == 1 && _button == false)  {
			obj.transform.SendMessage ("hitByRaycast");
			_button = true;
		}
		else if (device.GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x == 0 && _button == true){
			_button = false;
		}
	}
	//手把拖曳拼圖
	public void Puzzle_Draging (GameObject obj){
		if (device.GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x == 1) {
			if (obj.transform.tag == "Escape")
				SceneManager.LoadScene ("First");
			obj.transform.SendMessage ("VR_Draging");
			if (_button == false) {
				_button = true;
				obj.transform.SendMessage ("bigger",obj);
				biggerFirst = true;
			}

		}
		else if (device.GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x < 1 && biggerFirst == true) {
			if (_button == true) {
				obj.transform.SendMessage ("Smaller",obj);
				_button = false;
				biggerFirst = false;
			}
		}
	}
		
	public void VR_Menu(string MenuString)
	{
		if (device.GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x == 1) {
			if (MenuString == "_KeepGame_button(VR)") {
				Time.timeScale = 1;
				VR_Escape_Children.SetActive (false);
			} else if (MenuString == "PC_toggle(VR)") {
				memoryGame_Control.PC_DeviceChanging ();
			} else if (MenuString == "VR_toggle(VR)") {
				memoryGame_Control.VR_DeviceChanging ();
			}
		}
	}


}
