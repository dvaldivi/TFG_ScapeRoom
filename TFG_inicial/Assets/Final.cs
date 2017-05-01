using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (this.gameObject.GetComponent<botones_padre>().activada) {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController2>().Function_win();
        }
	}
}
