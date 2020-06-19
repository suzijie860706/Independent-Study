using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayPlayerCam : MonoBehaviour
{
    Ray ray; //射線
    float rayLenght = 4.5f; //射線的最大長度
    RaycastHit hit; //被射線打到的物件

    void Start()
    {
		
    }
    void Update()
    {
		//設定射線在攝影機的正中間
        ray = Camera.main.ScreenPointToRay
			(new Vector3(Screen.width / 2, Screen.height / 2, 0));
		//(射線,被射線打到的物件,射線長度)
        if (Physics.Raycast(ray, out hit, rayLenght)){
            hit.transform.SendMessage("hitByRaycast",
				gameObject,
				SendMessageOptions.DontRequireReceiver);
            //對被打中的物件呼叫 hitByRaycast 副程式，不回傳數字
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            //測試線
            //print(hit.transform.name);
            //測試hit物件
        }

    }

}
