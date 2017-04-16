using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gotera : MonoBehaviour {
    public GameObject gota;
    public float time;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (time > 1) {
            GameObject temp = (GameObject) Instantiate(gota, this.transform.position,this.transform.rotation);
            temp.transform.parent = this.transform;
            temp.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(1, 0, 6));
            time = 0;
        }
	}
}
