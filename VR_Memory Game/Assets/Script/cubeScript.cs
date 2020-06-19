using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeScript : MonoBehaviour {
	public MemoryGame_Control memoryGame_Control;
    Renderer R1;
    // Use this for initialization
    void Start()
    {
        R1 = gameObject.GetComponent<Renderer>();
        // Update is called once per frame
    }
	void Update () {

    }
    void hitByRaycast()
        {
		if (Input.GetKeyUp(KeyCode.E))
            {
			R1.material.color = Color.red;
				
            }
        }
}
