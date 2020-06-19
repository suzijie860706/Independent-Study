using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Control : MonoBehaviour {
	//音效
	public enum SE
	{
		NONE = -1,

		GRAB = 0,	//抓住拼圖時
		RELEASE,	//放開拼圖（不正確的情況）

		ATTACH,		//放開拼圖(正確)

		COMPLETE,   //完成拼圖時的音效

		BUTTON,		//ＧＵＩ按紐
	};
	public AudioClip[] audio_clips = new AudioClip[5];

	//完成判斷
	public int piece_finished_num = 0;

	//完成圖
	public GameObject complete_image;
	public GameObject VR_complete_image;
	//更換材質
	public Material material_origin;	//;缺主機板
	public Material material_next;	//缺各式零件

	//重新按鈕
	public GameObject Prafab_Rebutton;
	//設備控制
	public bool _VR = false;

	public GameObject _camera; // 電腦鏡頭
	public GameObject _pc_Prefab; // 預設圖
	public GameObject vr_Device; //VRTK

	//儲存
	private string pre_device;

	void Awake(){
		//讀檔
		Load ();
		//判斷裝置
		if (pre_device != "") {
			if (pre_device == "PC") {
				_VR = false;
			} else if (pre_device == "VR") {
				_VR = true;
			}	
		} else if (pre_device == null) {
			_VR = false;
		}
		//設定裝置
		CheckDevice();
	}

	void Start () {
	}

	void Update () {

	}
	//啟用何種裝置
	public void CheckDevice(){
		if (_VR == false) {
			Instantiate (_pc_Prefab, new Vector3 (0, 0, 0), Quaternion.Euler (0, 0, 0));
			vr_Device.SetActive (false);
		} else if (_VR == true) {
			Instantiate (_pc_Prefab, new Vector3 (6.49f, 0, 0), Quaternion.Euler (-90f, 0, 0));
			_camera.SetActive (false);
		}
	}
	public void Save(string _device){
		PlayerPrefs.SetString ("_device", _device);
	}
	public void Load(){
		pre_device = PlayerPrefs.GetString ("_device");
	}
	//播放音樂
	public void PlaySE(SE se ){
		this.GetComponent<AudioSource> ().PlayOneShot (audio_clips [(int)se],0.35f);
	}

	//完成拼圖+1
	public void Add_piece_finished_num(int num){
		piece_finished_num += 1;
	}
	public void complete_Action(){
		if (_VR == false)
			complete_image.GetComponent<Image> ().enabled = true;
		else if (_VR == true)
			VR_complete_image.SetActive (true);
		MemoryGame_Control.Back = true;
		Invoke ("BackPrimaryGame", 2.5f);
	}
	//重新開始
	public void ReStart_Button(){
		GameObject _PC = GameObject.Find ("PC");
		Destroy (_PC);

		_PC = Instantiate (Prafab_Rebutton);
		_PC.name = "PC";
	}
	//回到翻牌場景
	public void BackPrimaryGame(){
		UnityEngine.SceneManagement.SceneManager.LoadScene("First");

	}

}
