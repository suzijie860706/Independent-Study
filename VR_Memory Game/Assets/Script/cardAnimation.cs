using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class cardAnimation : MonoBehaviour {
	private MemoryGame_Control memoryGame_Control;
	private startGame _startGame;
	public static int _countScore = 0; 		//分數
	public static int _countTime = 0; 		//時間
	public static int CountCard = 0;  	// 0:紀錄第一張牌 1:記錄第二張
	//動畫控制
	private static Animation AnimONE ;	//紀錄第一招牌動畫
	private static Animation AnimTWO;	//記錄第二張牌動畫 2019/04/09
	private static GameObject _Card1 ;	//第一張牌
	private static GameObject _Card2 ;	//第一張牌
	private static string first_Number = null;	//紀錄牌1的數字
	private static string second_Number = null;	//紀錄牌2的數字

	//滑鼠按鍵事件
	EventTrigger trigger ;
	EventTrigger.Entry entry;

	//bool _button = false;
		
	void Start () {
		memoryGame_Control = GameObject.Find ("GameManager").GetComponent<MemoryGame_Control> ();

		_startGame = memoryGame_Control._startGame;
		Event_PointClick ();

    }

	//被雷射打到,呼叫副程式
    public void hitByRaycast()
	{
		if ((Input.GetKeyUp (KeyCode.E)	||	Input.GetKeyUp (KeyCode.Mouse0) || 
			memoryGame_Control._VR )
			&&  MemoryGame_Control.gameSwitch == true) {
			//當按下 鍵盤按鍵"E"時
			switch (CountCard)
			{
			case 0:
				//翻牌
				TurnOverCard ();
				//紀錄第一張數字
				first_Number = transform.GetChild (0).gameObject.tag;
				//紀錄第一張牌
				_Card1 = gameObject;
				//取消第一張牌的翻牌動作
				_Card1.GetComponent <cardAnimation> ().enabled = false;
				_Card1.GetComponent <BoxCollider> ().enabled = false;
				_Card1.GetComponent<EventTrigger> ().enabled = false;
				CountCard++;
				break;

			case 1:
				TurnOverCard ();
				second_Number = transform.GetChild (0).gameObject.tag;
				_Card2 = gameObject;
				//取消第一張牌的翻牌動作
				_Card2.GetComponent <cardAnimation> ().enabled = false;
				_Card2.GetComponent <BoxCollider> ().enabled = false;
				_Card2.GetComponent<EventTrigger> ().enabled = false;
				CountCard++;
				Invoke ("CleanTimesUp",0.7f);//0.7秒後翻回牌
				CheckScore(); //判斷加分
				if (_startGame.scoreText.text == "分數：8" ||	_startGame.VR_scoreText.text == "分數：8") {
					_startGame.CanceltimeRepeating ();
				}
				break;
			case 2:
				clean ();
				goto case 0;
				//break;
			}
		}
	}

	//當按下滑鼠左鍵時
	public void PointClick(BaseEventData data){
		hitByRaycast ();
	}

	//賦予滑鼠按鍵事件
	void Event_PointClick(){
		UnityEngine.Events.UnityAction<BaseEventData> callback = new UnityEngine.Events.UnityAction<BaseEventData> (PointClick);
		entry = new EventTrigger.Entry ();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback.AddListener (callback);
		trigger = gameObject.AddComponent<EventTrigger> ();
		trigger.triggers.Add (entry);

	}
	//按下E翻牌
	void TurnOverCard()
	{
		gameObject.GetComponent<Animation> ().Play ("TurnOverTheCard");//播放動畫
		//gameObject.GetComponent<AudioSource> ().PlayOneShot();//播放音效
		_startGame.turnCardAudio();
	}

	//時間到自動翻牌，讓變數歸於初始值
	void CleanTimesUp(){
		if (CountCard == 2) clean();
	}

	//時間未到，讓變數歸於初始值
	void clean(){
		if (first_Number != second_Number) {
			//將翻過的牌翻回
			_Card1.GetComponent<Animation> ().Play ("TurnOffTheCard");
			_Card2.GetComponent<Animation> ().Play ("TurnOffTheCard");

			_Card1.GetComponent <cardAnimation> ().enabled = true;//恢復第一張牌的翻牌動作
			_Card1.GetComponent <BoxCollider> ().enabled = true; //恢復第一張牌的翻牌動作
			_Card2.GetComponent <cardAnimation> ().enabled = true;
			_Card2.GetComponent <BoxCollider> ().enabled = true;
			_Card1.GetComponent<EventTrigger> ().enabled = true;
			_Card2.GetComponent<EventTrigger> ().enabled = true;
		}
		first_Number = null;
		second_Number = null;
		CountCard = 0;
	}
	//判斷牌是否同一張，是則加分
	void CheckScore(){
		if (first_Number == second_Number) {
			//加分
			InvisableCard (); //牌消失
			if (memoryGame_Control._VR == false)
				_startGame.scoreText.text = "分數：" + ++cardAnimation._countScore;
			else
				_startGame.VR_scoreText.text = "分數：" + ++cardAnimation._countScore;
		}
	}
	//當牌正確，清除牌的可翻姓
	void InvisableCard(){
		_Card1.GetComponent<cardAnimation> ().enabled = false;
		_Card2.GetComponent<cardAnimation> ().enabled = false;
		_Card1.GetComponent<EventTrigger> ().enabled = false;
		_Card2.GetComponent<EventTrigger> ().enabled = false;
		_Card1.GetComponent <BoxCollider> ().enabled = false;
		_Card2.GetComponent <BoxCollider> ().enabled = false;
	}
	//====================VR部分======================
	private void OnTriggerStay(Collider collider){
		if (memoryGame_Control._VR == true) {
			VR_Control.Control_right.transform.SendMessage ("Check", gameObject, SendMessageOptions.DontRequireReceiver);
		}
	}

}

