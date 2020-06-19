using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
public class MemoryGame_Control : MonoBehaviour {
	public static bool gameSwitch = false; //遊戲開關
	public startGame _startGame;

	public bool GameEnd = false;//遊戲是否中途結束

	public int countScore; // 初始化分數
	public int countTime; //初始化時間

	//載入回原場景後 判斷主機圖片是否出現
	public static bool Back = false;
	//相機控制
	public FirstPersonController _firstPersonController;
	//VR控制
	public GameObject _VRTK; 
	public GameObject _characterObject;
	public bool _VR = false; // 電腦 VR 轉換
	public VR_Control vr_control; // 開啟關閉腳本

	//Escape 介面
	public GameObject _escape;
	public GameObject VR_escape;
	public Toggle pc_toggle;
	public Toggle vr_toggle;
	//Escape 介面 (VR)版本
	public Toggle pc_toggle_VR;
	public Toggle vr_toggle_VR;
	//儲存
	private string pre_device;
	public GameObject ar_Picture;

	void Awake(){
		//讀檔
		Load ();
		//------------判斷裝置---------------
		if (pre_device != "") {
			if (pre_device == "PC") {
				_VR = false;
			} else if (pre_device == "VR") {
				_VR = true;
			}	
		} else if (pre_device == null) {
			_VR = false;
		}
		//-------------設定裝置---------------
		if (_VR == true){
			vr_toggle_VR.isOn = true;
			pc_toggle_VR.isOn = false;
			vr_control.enabled = true; 		//VR腳本控制開啟
			_VRTK.SetActive (true);
			_characterObject.SetActive (false); //PC裝置關閉
			_startGame.timeText.enabled = false;
			_startGame.scoreText.enabled = false;
			_startGame._timeImgae.SetActive (false);
			_startGame._rawImage.SetActive (false);
			_startGame.StartTip.SetActive (false);
			_startGame.press_mouse0.SetActive (false);
			//-----遊戲開始才出現分數跟時間-----
			_startGame.VR_timeText.enabled = false;
			_startGame.VR_scoreText.enabled = false;
			_startGame.VR_Time_image.SetActive (false);
			_startGame.VR_press.SetActive(false);
		}
		else if (_VR == false){
			vr_toggle.isOn = false;
			pc_toggle.isOn = true;
			vr_control.enabled = false;
			_characterObject.SetActive (true); //PC裝置關閉
			_VRTK.SetActive (false);
			_startGame.VR_timeText.enabled = false;
			_startGame.VR_scoreText.enabled = false;
			_startGame.VR_Time_image.SetActive (false);
			_startGame.VR_StartTip.SetActive (false);
			Destroy (_startGame.VR_press);

			//-----遊戲開始才出現分數跟時間-----
			_startGame.timeText.enabled = false;
			_startGame.scoreText.enabled = false;
			_startGame._timeImgae.SetActive (false);
		}

		//--------------如果完成拼圖--------------
		if (Back == true) {
			ar_Picture.SetActive (true);
			_startGame.StartTip.SetActive (false);
			_startGame._rawImage.SetActive (false);
		}
	}

	void Start () {

	}

	void Update () {
		if (_VR == true) {
			VR_escape.GetComponent<BoxCollider> ().enabled = false;
		}
		if (_VR == false) {
			
			if (Input.GetKeyUp (KeyCode.Escape)) {
				//打開選單
				if (_escape.activeSelf == false) {
					_escape.SetActive (true);
					Time.timeScale = 0;
					if (gameSwitch == false)_firstPersonController.enabled = false;
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;

				//如果已經打開，則關閉他
				} else if (_escape.activeSelf == true){
					_escape.SetActive (false);
					Time.timeScale = 1;
					if (gameSwitch == false)_firstPersonController.enabled = true;
					Cursor.lockState = CursorLockMode.Locked;
					Cursor.visible = false;
				}


			}
		} 

		if (Input.GetKeyDown (KeyCode.F6))  {
				PlayerPrefs.SetString ("_device", "PC");
		}
	}
	//按下繼續遊戲
	public void KeepGame(){
		Time.timeScale = 1;
		_firstPersonController.enabled = true;
		_escape.SetActive (false);
	}

	//切換裝置 VR OR PC
	public void VR_DeviceChanging(){
		if (_VR == false) {
			if (vr_toggle.isOn)
				pc_toggle.isOn = false;
			else if (pc_toggle.isOn == false)
				vr_toggle.isOn = true;

		} 
		else if (_VR == true) {

			if (vr_toggle_VR.isOn) {
				pc_toggle_VR.isOn = false;
			}
			else if (pc_toggle_VR.isOn == false)
				vr_toggle_VR.isOn = true;
		}
		Save ("VR");

	}
	//切換裝置 VR PC
	public void PC_DeviceChanging(){
		if (_VR == false) {
			if (pc_toggle.isOn) {
				vr_toggle.isOn = false;
			}
			else if (vr_toggle.isOn == false)
				pc_toggle.isOn = true;
		}
		else if (_VR == true) {

			if (pc_toggle_VR.isOn) {
				vr_toggle_VR.isOn = false;
			}
			else if (vr_toggle_VR.isOn == false)
				pc_toggle_VR.isOn = true;
		}
		Save ("PC");
	}
	public void Save(string _device){
		PlayerPrefs.SetString ("_device", _device);
	}
	public void Load(){
		pre_device = PlayerPrefs.GetString ("_device");

	}

}
