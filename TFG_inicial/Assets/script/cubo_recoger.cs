using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubo_recoger : MonoBehaviour {
    public Material normal;
    public Material rojo;
    private Material mimaterial;
    private float original_y;
    private float factor;
    public bool torre;
    public GameObject mitorre;
    // Use this for initialization
    void Start () {
        factor = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void hover()
    {
       
        GetComponent<Renderer>().material = rojo;

        
    }

    public void unhover()
    {
       
        GetComponent<Renderer>().material = normal;
    }

    public void coge(Transform transform_player)
    {
        factor = 1;
        original_y = transform.position.y;
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        if (mitorre != null) {
            mitorre.gameObject.GetComponent<torre_eq>().quita_cubo();
            this.transform.parent = null;
        }
        mitorre = null;
        torre = false;
        
    }

    public void mueve(Transform transform_player)
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && factor < 1.3f) {
            factor += 0.1f;

        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && factor >0.3f)
        {
            factor -= 0.1f;

        }
        this.transform.position = transform_player.position + transform_player.forward * 60*factor;
        if (this.transform.position.y < original_y) {

            this.transform.position = new Vector3(this.transform.position.x,original_y, this.transform.position.z);
        }
    }

    public void suelta()
    {
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<Renderer>().material = normal;
        if (mitorre != null) {
            if (Vector3.Distance(this.transform.position, mitorre.transform.position) < 50)
            {
                this.transform.position = new Vector3(mitorre.transform.position.x, this.transform.position.y, mitorre.transform.position.z);
                torre = true;
                mitorre.gameObject.GetComponent<torre_eq>().añade_cubo();
                this.transform.parent = mitorre.transform.parent.transform;
                this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            }
            else {
                torre = false;
                mitorre = null;
                this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        mitorre = other.gameObject;
    }
}
