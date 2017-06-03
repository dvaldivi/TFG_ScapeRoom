using UnityEngine;
using System.Collections;

public class Trigger_LoadScene : MonoBehaviour {
	public string sceneName;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider user){
		if (user.tag == "Player") {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().win = true;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().gameOver = true;
			
		}
	}
}
