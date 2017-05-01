using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController2 : MonoBehaviour {
    public float time;
    public bool win;
	// Use this for initialization
	void Start () {
        win = false;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (win) {
            this.GetComponent<Recopilador_informacion>().envia_toda_info();
        }

	}

    public void Function_win() {
        this.win = true;

    }
}
