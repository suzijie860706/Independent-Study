using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class sample : MonoBehaviour 
{


   [SerializeField] private Canvas customUI;
   void OnTriggerEnter(Collider GameController)
   {
	customUI.enabled = true;
   }
   void OnTriggerExit(Collider GameController)
   {
	customUI.enabled = false;
   }
}