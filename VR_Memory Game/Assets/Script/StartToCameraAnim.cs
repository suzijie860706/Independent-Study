using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
public class StartToCameraAnim : MonoBehaviour {


	public MemoryGame_Control memoryGame_Control;
	public startGame _startGame;
	public GameObject VRTK_vector;
	float Dist;//計算距離
	Vector3 positionA = new Vector3(-3.7f,3.6f,5.638f);//開始遊戲位置
	Vector3 VR_positionA = new Vector3(-3.7f,3.6f,3.638f);//VR開始遊戲位置
	Quaternion rotationA = Quaternion.Euler(86.5f,0,0);//開始遊戲角度

	MouseLook _lock = new MouseLook();//鎖定鏡頭
	public Texture2D defaultTexture;
	public RawImage _RawImage;
	public Vector2 hotspot = Vector2.zero;
	//轉回遊戲人物資料
	bool C = false;
	Vector3 positionB;
	Quaternion rotationB;
	//VR 鏡頭

	void Start () {

		if (memoryGame_Control._VR == true) {
			StartToCameraAnim _VR_Camera = GameObject.Find ("Camera (eye)").GetComponent<StartToCameraAnim>();
		}
	}
	void Update () {
		if (memoryGame_Control.GameEnd == false) {
			PositionChanging (); 
			if (C == false) {
				positionB = transform.position;
				rotationB = transform.rotation;
				C = true;
			}
		} 
		else {
			PositionToCharacter ();
		}
	}
	public void PositionChanging(){//鏡頭位置轉換
		if (memoryGame_Control._VR == false) {
			//避免動畫過程中可以上下左右
			gameObject.GetComponentInParent<FirstPersonController>().enabled = false;
			transform.position = Vector3.Lerp (transform.position, positionA, 2*Time.deltaTime);//漸漸拉近距離
			transform.rotation = Quaternion.Lerp (transform.rotation, rotationA, 2*Time.deltaTime);//漸漸拉近角度

			Dist = Vector3.Distance (positionA, transform.position);
			if (Dist < 0.005f) {
				//切換攝影機
				GetComponent<StartToCameraAnim> ().enabled = false;
				GetComponent<RayPlayerCam> ().enabled = false;
				//滑鼠鎖定取消

				_lock.SetCursorLock (false);//不鎖定滑鼠
				Cursor.SetCursor (defaultTexture, hotspot, CursorMode.Auto);
				_RawImage.enabled = false;
				_startGame.StartGame ();
			}
		} else {
			
			VRTK_vector.transform.position = Vector3.Lerp (VRTK_vector.transform.position, VR_positionA, 2*Time.deltaTime);//漸漸拉近距離
			Dist = Vector3.Distance (VR_positionA,VRTK_vector.transform.position);
			if (Dist < 0.05f) {


				gameObject.GetComponent<StartToCameraAnim> ().enabled = false;
				_startGame.StartGame ();
			}
		}

	}
	public void PositionToCharacter(){
		//轉回遊戲人物
		if (memoryGame_Control._VR == false) {
			transform.position = Vector3.Lerp (transform.position, positionB, 2*Time.deltaTime);//漸漸拉近距離
			transform.rotation = Quaternion.Lerp (transform.rotation, rotationB, 2*Time.deltaTime);//漸漸拉近角度

			Dist = Vector3.Distance (positionB, transform.position);
			if (Dist < 0.005f) {
				_startGame.timeText.enabled = false;
				_startGame.scoreText.enabled = false;
				_startGame._timeImgae.SetActive (false);
				//切換攝影機
				gameObject.GetComponentInParent<FirstPersonController>().enabled = true;
				_lock.SetCursorLock (true);//鎖定滑鼠
				_RawImage.enabled = true;
				GetComponent<RayPlayerCam> ().enabled = true;
				GetComponent<StartToCameraAnim> ().enabled = false;

			}
		} else {
			VRTK_vector.transform.position = Vector3.Lerp (VRTK_vector.transform.position, positionB, 2*Time.deltaTime);//漸漸拉近距離

			Dist = Vector3.Distance (positionB, VRTK_vector.transform.position);
			if (Dist < 0.005f) {
				_startGame.VR_timeText.enabled = false;
				_startGame.VR_scoreText.enabled = false;
				_startGame.VR_Time_image.SetActive (false);
				//切換攝影機
				GetComponent<StartToCameraAnim> ().enabled = false;
				_lock.SetCursorLock (true);//鎖定滑鼠
			}
		}
	}


}
