namespace VRTK.Examples
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.EventSystems;
	public class PC_Script : MonoBehaviour {
		public Control _control ;
		// 遊戲攝影機.
		public	GameObject	game_camera;

		//計算滑鼠的世界座標雷射
		GameObject plane;
		Vector3 world_Position ;
		Ray ray;

		enum STEP{
			NONE,	//無

			DRAGING,//拖拉

			ATTACH,//對準時
		}
		private STEP step = STEP.NONE;
		//當拖拉時，物件與滑鼠的差值
		Vector3 offset;

		//主機零件
		GameObject _MotherBoard = null ;
		//主機提示
		//顏色對位，變亮提示
		Color color = Color.white;

		//正確位置時放開拼圖判斷
		bool InTheRightPlace = false;

		//正確位置
		Vector3[] finished_Position = new Vector3[7];
		//VR
		private GameObject VR_Ray;
		Vector3 VR_RayPosition;

		void Start () {
			_control = GameObject.FindGameObjectWithTag ("GameController").GetComponent<Control>();
			//將所有物件的腳本賦予攝影機
			game_camera = _control._camera;
			//賦予變數實體物件

			_MotherBoard = GameObject.FindWithTag ("1");
			//零件完成位置
			PC_Finished_Position();
			if (_control._VR == false) {
			}
			VR_Ray = GameObject.Find ("Right");
		}
		void Update () {
			if (_control._VR == false) {
				//將滑鼠座標轉換為世界座標
				unproject_mouse_position (out world_Position, Input.mousePosition);
				plane = GameObject.Find ("Plane");
			} 
			else if (_control._VR == true) {
				VR_RayPosition = VR_Ray.GetComponent<VRTK_ControllerPointerEvents_ListenerExample> ()._VR_position;
				plane = GameObject.Find ("Plane");
			}

			//拼圖放開時，接到正確位置
			if (InTheRightPlace == true) {
				piece_Lerp ();
			}
			
			

		}
		//利用雷射與平面製造距離 算出滑鼠座標
		public bool unproject_mouse_position(
		out Vector3 world_position, Vector3 mouse_position){
			float depth;//長度D
			//從鏡頭到滑鼠射出的雷射
			ray = game_camera.GetComponent<Camera> 
				().ScreenPointToRay (Input.mousePosition);
			Plane plane = new Plane (Vector3.up, new Vector3
				(0f,transform.position.y,0f));

				if(plane.Raycast(ray,out depth)){
					world_position = 
					(ray.origin + ray.direction * depth);
					return (true);
				}
				else {
					world_position = Vector3.zero;
					return (false);
				}

		}
		//拼圖正確位置設定
		private void PC_Finished_Position()
		{
				if (_control._VR == false) {
					finished_Position [0] = new Vector3 (-7.730911f, 0.1f, 0.8808738f);
					finished_Position [1] = new Vector3 (-7.893809f, 0.1f, -1.873408f);
					finished_Position [2] = new Vector3 (-8.630849f, 0.1f, -0.619093f);
					finished_Position [3] = new Vector3 (-7.090007f, 0.1f, 1.642644f);
					finished_Position [4] = new Vector3 (-8.325084f, 0.1f, 3.078096f);
					finished_Position [5] = new Vector3 (-8.588122f, 0.1f, 0.3250548f);
					finished_Position [6] = new Vector3 (-8.402527f, 0.1f, 1.473913f);
				} else if (_control._VR == true) {
					finished_Position [0] = new Vector3 (-1.23f, 0.83f, -0.1f);
					finished_Position [1] = new Vector3 (-1.4f,-1.84f, -0.1f);
					finished_Position [2] = new Vector3 (-2.094f,-0.598f, -0.1f);
					finished_Position [3] = new Vector3 (-0.608f, 1.605f, -0.1f);
					finished_Position [4] = new Vector3 (-1.84f, 3.11f, -0.1f);
					finished_Position [5] = new Vector3 (-2.05f, 0.32f, -0.1f);
					finished_Position [6] = new Vector3 (-1.93f, 1.5f, -0.1f);
				}
		}
		//拖拉
		public void drag(){
			step = STEP.DRAGING;
			//物件拖拉
			gameObject.transform.position = world_Position + offset;

			//目前拉動的物件拚對的話......
			Rightplace();
		}
		//判斷哪一個物件
		private void Rightplace(){
			
			if (_MotherBoard.activeSelf == false) {
				//主機板以外
				if (Vector3.Distance (this.transform.position, finished_Position [int.Parse (tag) - 1]) < 0.35f) {
					judge(true);
				} else {
					judge (false);
				}
			} else {//主機板
				if (Vector3.Distance (this.transform.position, finished_Position [0]) < 0.35f) {
					judge(true);
				} else {
					judge (false);
				}
			}
		}

		//當拼圖碎片靠近正確位置一定距離時 發亮提示
		void judge(bool I){	
			if (I == true) {
				gameObject.GetComponent<SpriteRenderer> ().material.color = color * 1.1f;
				step = STEP.ATTACH;
			} else if (I == false) {
				gameObject.GetComponent<SpriteRenderer> ().material.color = color;
				step = STEP.DRAGING;
			}
		}

		private void FinishPazzle()
		{
			//拼圖數量+1
			_control.Add_piece_finished_num (1);
			if (_control.piece_finished_num == 7) {
				Invoke ("COMPLETE", 1f);

			}
			//使該片拼圖失效不能繼續拼
			gameObject.GetComponent<EventTrigger> ().enabled = false;
			gameObject.GetComponent<BoxCollider> ().enabled = false;
			//將顏色調回正常
			gameObject.GetComponent<SpriteRenderer> ().material.color = color;
		}
		public void PointDown()
		{
			//拖拉前的前置動作，計算差值
			offset = this.gameObject.transform.position - world_Position;
			//放大
			gameObject.transform.localScale += new Vector3 (0.04f, 0.04f, 0f);
			Destroy (gameObject.GetComponentInChildren<TextMesh> ());
			_control.PlaySE(Control.SE.GRAB);
		}
		public void PointUp(){
			//縮小
			gameObject.transform.localScale -= new Vector3 (0.04f, 0.04f, 0f);

			//當放開拼圖，且在正確位置
			if (step == STEP.ATTACH) {
				InTheRightPlace = true;
				//完成一片拼圖時的動作
				FinishPazzle ();


				_control.PlaySE (Control.SE.ATTACH);
			} else
				_control.PlaySE (Control.SE.RELEASE);
		}
		void piece_Lerp(){//拼圖自動對齊
			gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, finished_Position [int.Parse(tag) - 1],2 *Time.deltaTime ) ;
			//當主機板距離為0時
			if (gameObject.transform.position == finished_Position [int.Parse (tag) - 1]) {
				InTheRightPlace = false;
			}
			//對齊後 判斷是否為主機板拼上 換另一張預備好的圖
			After_piece();
		}

		//將拼圖拼到正確位置
		void After_piece(){
				if (_control._VR == false) {
				if (_MotherBoard.transform.position == new Vector3 (-7.730911f, 0.1f, 0.8808738f)) {
					plane.GetComponent<MeshRenderer> ().material.CopyPropertiesFromMaterial (_control.material_next);
					Destroy (GameObject.Find ("Computer Cabinet_Text"));
					_MotherBoard.SetActive (false);

					}

				}
				if (_control._VR == true) {
				if (_MotherBoard.transform.position == new Vector3 (-1.23f, 0.83f, -0.1f)) {
					plane.GetComponent<MeshRenderer> ().material.CopyPropertiesFromMaterial (_control.material_next);
					Destroy (GameObject.Find ("Computer Cabinet_Text"));
					_MotherBoard.SetActive (false);
					}
				}


		}
		//拼圖全部拚完成，過完一秒執行此
		public void COMPLETE(){
		    _control.PlaySE (Control.SE.COMPLETE);
			_control.complete_Action ();
		}
			//呼叫VR設備
		private void OnTriggerStay(Collider collider){
			if (_control._VR == true ) {
				VR_Control.Control_right.transform.SendMessage ("Puzzle_Draging", gameObject, SendMessageOptions.DontRequireReceiver);
			}
		}
		public void PuzzleOnRay(){
			if (step == STEP.DRAGING) {
				gameObject.transform.position = new Vector3 (VR_RayPosition.x, VR_RayPosition.y, -0.1f);
			}

		}
			//板機拖拉
		public void VR_Draging(){
			step = STEP.DRAGING;	
			gameObject.transform.position = new Vector3(VR_RayPosition.x,VR_RayPosition.y,-0.1f);
			//目前拉動的物件拚對的話......
			Rightplace();
		}

		public void bigger(GameObject obj){
			//取消其他碰撞
			if (obj.transform.tag != _MotherBoard.transform.tag) {
				_MotherBoard.GetComponent<BoxCollider> ().enabled = false;
			}
			for (int i = 2; i <= 7; i++) {
				if (i != int.Parse(obj.transform.tag)){
					GameObject.FindGameObjectWithTag (i.ToString()).GetComponent<BoxCollider>().enabled = false;
				}
			}

			InvokeRepeating ("PuzzleOnRay", 0, 0.01f); // 讓拼圖隨時跟隨雷射
			gameObject.transform.localScale += new Vector3 (0.04f, 0.04f, 0f);//放大
			Destroy (gameObject.GetComponentInChildren<TextMesh> ());
			_control.PlaySE(Control.SE.GRAB);
		}
		public void Smaller(GameObject obj){
//			開啟其他碰撞
			if (obj.transform.tag != _MotherBoard.transform.tag) {
				_MotherBoard.GetComponent<BoxCollider> ().enabled = true;
			}
			for (int i = 2; i <= 7; i++) {
				if (i != int.Parse(obj.transform.tag)){
					GameObject.FindGameObjectWithTag (i.ToString()).GetComponent<BoxCollider>().enabled = true;
				}
			}

			CancelInvoke ("PuzzleOnRay");// 取消拼圖隨時跟隨雷射
			gameObject.transform.localScale -= new Vector3 (0.04f, 0.04f, 0f);//縮小
			//當放開拼圖，且在正確位置
			if (step == STEP.ATTACH) {
				InTheRightPlace = true;
				FinishPazzle ();//完成一片拼圖時的動作
				_control.PlaySE (Control.SE.ATTACH);
			} else
				_control.PlaySE (Control.SE.RELEASE);
				step = STEP.NONE;

		}
	}	
}
