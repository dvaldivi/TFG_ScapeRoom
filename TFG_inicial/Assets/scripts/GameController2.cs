using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController2 : MonoBehaviour {
    public float time;
    public bool win;
    public bool lose;
    // Use this for initialization
    void Start () {
        win = false;
        lose = false;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (win) {
            this.GetComponent<Vr_Tracker>().send_info("win");
        }
        else if (lose) {
            this.GetComponent<Vr_Tracker>().send_info("lose");
        }

	}

    public void Function_win() {
        this.win = true;

    }
    void envia_info_de_todos_modos()
    {
        if (!lose && !win) {
            this.GetComponent<Vr_Tracker>().send_info("disconected");
        }
        
    }
}
