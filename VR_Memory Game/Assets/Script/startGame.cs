using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class startGame : MonoBehaviour {
	


	public MemoryGame_Control memoryGame_Control;
	public StartToCameraAnim cameraChange;
	//UI介面控制
	public Text timeText; //顯示時間文字
	public Text scoreText; //顯示分數文字
	public GameObject _rawImage;
	public GameObject _timeImgae;
	public GameObject StartTip; //開始遊戲提示
	public GameObject press_mouse0; //提示左鍵
	int press_mouse0_count = 0;//提示閃爍
	//VR 介面控制
	public GameObject VR_camera; 		//VR攝影機
	public Text VR_timeText; 			//顯示時間文字
	public Text VR_scoreText; 		 	//顯示分數文字
	public GameObject VR_Time_image; 	//時間圖片
	public GameObject VR_StartTip;		//開始遊戲提示
	public GameObject VR_press;			//翻牌提示

	//洗牌變數、陣列
	public GameObject _backCard; 					//預設牌
	int[] No_Card = new int[16]; 					//洗亂用的數字陣列
	GameObject[] BackCards = new GameObject[16];	//洗牌後，儲存每一張卡牌的陣列
	Quaternion Quaternion_Card = Quaternion.Euler(-90,0,180);//產生牌的預設角度
	//電腦零件的部分
	GameObject[] pc_Components_array = new GameObject[16]; //洗牌後，儲存每一張零件的陣列

	private Animation[] AnimC = new Animation[16];

	//子圖片
	private GameObject[] computer = new GameObject[8];
	public GameObject computeCabinet;
	public GameObject power;
	public GameObject hardDisk;
	public GameObject motherBoard;
	public GameObject CPU;
	public GameObject memory;
	public GameObject graphicsCard;
	public GameObject nic;
	//音效
	public AudioSource _audioSource;
	public AudioClip _failure;
	public AudioClip _successful;
	public AudioClip _TurnOverTheCard;
	void Start () {

		for (int number = 0; number <= 15; number++) 
		{ //將陣列賦值
			No_Card [number] = ((number + 2)/ 2); // (i+2)/2 方便日後兩兩歸一組
		}
		computer = new GameObject[]{computeCabinet, power, hardDisk, motherBoard, CPU, memory, graphicsCard, nic};
		//如果不是VR 則關掉VR的顯示
		if (memoryGame_Control._VR == false) {
			memoryGame_Control.vr_control.enabled = false;
			VR_scoreText.enabled = false;
			VR_timeText.enabled = false;
			VR_StartTip.SetActive (false); //遊戲提示
			VR_Time_image.SetActive(false);
		}
    }
    public void hitByRaycast()
	{//當遊戲看下開始鍵時,遊戲鏡頭轉變
		if ((Input.GetKeyUp (KeyCode.E)	||	Input.GetKeyUp (KeyCode.Mouse0)	||  memoryGame_Control._VR )
			&& MemoryGame_Control.gameSwitch == false){
			//重玩判斷
			if (memoryGame_Control.GameEnd == true) {
				memoryGame_Control.GameEnd = false;
				ResetCard ();
			}
			gameObject.SetActive(false);			//遊戲開始 關閉提示
			StartTip.SetActive (false);				//開始遊戲提示關閉
			VR_StartTip.SetActive (false);			//VR開始提示關閉

			InvokeRepeating ("press_mouse0_Switch", 5.5f, 2);//提示Ｅ鍵
			if (memoryGame_Control._VR == false) cameraChange.enabled = true;
			else  VR_camera.GetComponent<StartToCameraAnim> ().enabled = true;
		}
    }

	public void StartGame()
	{
		Initialize_StartGame();//遊戲開始,初始化變數 洗牌
		if (memoryGame_Control._VR == false) {
			timeText.enabled = true;
			scoreText.enabled = true;
			_timeImgae.SetActive (true);
		} else if (memoryGame_Control._VR == true) {
			VR_timeText.enabled = true;
			VR_scoreText.enabled = true;
			VR_Time_image.SetActive (true);
		}


			//將陣列運用shuffle算法洗亂
			for (int Number = 15; Number >= 0; Number--) {
			//隨機交換陣列數值
				int X = Random.Range (0, Number);
				int temp = No_Card [X];
				No_Card [X] = No_Card [Number];
				No_Card [Number] = temp;
			}
			
			//放上洗好的牌 
			GameObject cardName; 		//用來命名創造的卡牌
			GameObject pc_components; 	//用來命名電腦設備卡牌
			for (int i = 0; i <= 15; i++) {
				GameObject Card = GameObject.Find ("BackCard" + i.ToString ());
				Vector3 Vector3_Card = Card.transform.position;							//BackCard[i]卡片座標
				cardName = Instantiate (_backCard, Vector3_Card, Quaternion_Card);
				cardName.name = "Card" + ((No_Card [i] + 1).ToString ());			//幫卡牌命名
				cardName.GetComponent<Animation>().Play("TurnOverTheCard");
				Destroy (Card);//將原本背面牌刪除
			//零件
				Vector3 Vector3_Object = new Vector3(Vector3_Card.x,Vector3_Card.y - 0.0001f ,Vector3_Card.z);
				pc_components = Instantiate (computer[No_Card[i]-1],Vector3_Object,Quaternion.Euler(270,0,180),cardName.transform);
			//
				BackCards [i] = cardName;
				pc_Components_array [i] = pc_components;
				AnimC [i] = cardName.GetComponent<Animation> ();//翻回牌用
		}
			Invoke ("TurnOffCard", 2.5f);//將牌翻回
	}
	//初始化分數及時間
	public void Initialize_StartGame(){
		cardAnimation._countTime = memoryGame_Control.countTime;
		cardAnimation._countScore = memoryGame_Control.countScore;

		if (memoryGame_Control._VR == false) {
			timeText.text = cardAnimation._countTime.ToString();//起始時間
			scoreText.text = "分數：" + cardAnimation._countScore;

		} else {
			VR_timeText.text = cardAnimation._countTime.ToString();//起始時間
			VR_scoreText.text = "分數：" + cardAnimation._countScore;

		}

	}
	//遊戲勝利時
	public void CanceltimeRepeating(){
		CancelInvoke ("timeRepeating");//停止計時
		gameObject.SetActive (true); 
		MemoryGame_Control.gameSwitch = false;//遊戲結束判斷
		memoryGame_Control.GameEnd = true;//判斷是否過關
		_audioSource.PlayOneShot(_successful,4f);//音效

		if (memoryGame_Control._VR == false) {
			cameraChange.enabled = true;
			StartTip.GetComponent<Text>().text = "請進入螢幕完成拼圖";
			StartTip.SetActive (true);
		}
		else if(memoryGame_Control._VR == true){//VR開關
			GameObject VR_camera = GameObject.Find("Camera (eye)");
			VR_camera.GetComponent<StartToCameraAnim> ().enabled = true;
			VR_StartTip.GetComponent<Text>().text = "請對準電腦扣下板機進入，完成拼圖";
			VR_StartTip.SetActive (true);
		}


	}
	//當秒數為0，停止計時
	private void Timesup(){
		CancelInvoke("timeRepeating"); //取消注入副程式執行
		MemoryGame_Control.gameSwitch = false;
		gameObject.SetActive (true); 
		ResetCard ();
	}
	//時間倒數
	private void timeRepeating()
	{

		if (memoryGame_Control._VR == false) {
			timeText.text = (--cardAnimation._countTime).ToString ();		//每秒減1
		}
		else if(memoryGame_Control._VR == true){
			VR_timeText.text = (--cardAnimation._countTime).ToString ();	//每秒減1
		}

		if (cardAnimation._countTime == 0) {
			Timesup ();
			_audioSource.PlayOneShot (_failure,4f);
		}

	}
	//將牌面回歸初始狀態
	private void ResetCard(){
		GameObject cardName; 	//用來命名創造的卡牌
		cardAnimation.CountCard = 0; //
		for (int i = 0; i <= 15; i++) {
			Destroy (BackCards [i]);
			Destroy (pc_Components_array [i]);
			Vector3 Vector3_BackCards = BackCards [i].transform.position;
			cardName = Instantiate (_backCard, Vector3_BackCards, Quaternion.Euler (-451.781f, 90f, 90f));
			cardName.name = "BackCard" + i.ToString ();

		}
	}
	//按下左鍵點擊牌提示
	private void press_mouse0_Switch()
	{
		if (press_mouse0_count <= 1) {
			if (memoryGame_Control._VR == false) press_mouse0.SetActive (!press_mouse0.activeSelf);
			else VR_press.SetActive (!VR_press.activeSelf);

			press_mouse0_count++;
		}
		else  
			{
				CancelInvoke ("press_mouse0_Switch");
				press_mouse0_count = 0;
			}
		
	}
	//將全部的牌翻回去
	private void TurnOffCard()
	{
		for (int x = 0; x <= 15; x++)
		{
			AnimC[x].Play("TurnOffTheCard");

		}
		MemoryGame_Control.gameSwitch = true;//牌翻回去 遊戲判斷"是"
		InvokeRepeating ("timeRepeating", 1, 1); //牌翻回去 開始倒數時間
	}

	private void OnTriggerStay(Collider collider){
		if (memoryGame_Control._VR == true) {
			VR_Control.Control_right.SendMessage ("Check", gameObject, SendMessageOptions.DontRequireReceiver);
		}
	}
	public void turnCardAudio(){
		_audioSource.PlayOneShot(_TurnOverTheCard,30f);//音效
	}
}
