using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gota : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "vaso")
        {
            collision.collider.gameObject.GetComponent<Vaso>().llena();
        }
        else if (collision.collider.gameObject.tag == "nota")
        {
            collision.collider.gameObject.GetComponent<Nota_visible_agua>().llena();
        }
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "vaso")
        {
            other.gameObject.GetComponent<Vaso>().llena();
        }
        else if (other.gameObject.tag == "nota")
        {
            other.gameObject.GetComponent<Nota_visible_agua>().llena();
        }
        Destroy(this.gameObject);
    }
}
