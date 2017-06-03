using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VrTracker_login_Manager : MonoBehaviour {
    public string Login_url = "http://monitorizer.sytes.net:8000/api-token-auth/"; 
    public string Register_url = "http://monitorizer.sytes.net:8000/polls/users/register/"; 
    public string Check_AppCode_url = "http://monitorizer.sytes.net:8000/polls/class/checkAppCode";
    
    public Sprite logo;
    public Sprite Background;
    // Use this for initialization
    void Start()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.GetComponent<Authentication>()) {
                go.GetComponent<Authentication>().setParameters(Login_url, Register_url, Check_AppCode_url, "main");
            }
            if (go.name.Equals("Fondo")) {
                if (go.GetComponent<SpriteRenderer>()) {
                    go.GetComponent<SpriteRenderer>().sprite = Background;
                }
            }
            if (go.name.Equals("Logo"))
            {
                if (go.GetComponent<SpriteRenderer>())
                {
                    go.GetComponent<SpriteRenderer>().sprite = logo;
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
