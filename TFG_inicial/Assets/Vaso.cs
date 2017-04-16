using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vaso : MonoBehaviour {
    public  float cantidad;
    public GameObject gota;
    public GameObject gota_prefab;
    public Transform pos_goteo;
    public float time;
    // Use this for initialization
    void Start () {
        cantidad = 0;
        gota.transform.localScale = new Vector3(cantidad / 100, cantidad / 100, cantidad / 100);
    }
	
	// Update is called once per frame
	void Update () {
        gota.transform.RotateAroundLocal(new Vector3(1,0,1), 0.05f);
        if ( Input.GetKey(KeyCode.Space))
        {
            if (time > 0.2f)
            {
                derrama();
                time = 0;
                gota.transform.localScale = new Vector3((float)cantidad / 100, (float)cantidad / 100, (float)cantidad / 100);
            }
            
        }
        else {
            this.transform.localEulerAngles = new Vector3(-90, 0, 0);
        }

        time += Time.deltaTime;
    }
    public void llena() {
        if (cantidad < 200 && !Input.GetKey(KeyCode.Space))
        {
            cantidad += 10;
            
            
            gota.transform.localScale = new Vector3((float)cantidad / 100, (float)cantidad / 100, (float)cantidad / 100);
        }

    }

    private void derrama() {
        this.transform.localEulerAngles  = new Vector3(90,0,0);
        if (cantidad > 5)
        {
            cantidad -= 5;
        GameObject temp = (GameObject)Instantiate(gota_prefab, pos_goteo.transform.position, this.transform.rotation);
      
        temp.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(1, 0, 6));
            
        time = 0;
    }

    }
}
