using UnityEngine;
using System.Collections;
using System;

public class Reset : MonoBehaviour {
    private GameObject gameController;
	// Use this for initialization
	void Start () {
        gameController = GameObject.FindGameObjectWithTag("GameController");

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void unhover()
    {
        Debug.Log("hola");
    }

    public void hover()
    {
        Debug.Log("reseteo");
        gameController.GetComponent<GameController>().reset();
    }
}
