using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class antorcha : MonoBehaviour {

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.GetComponent<Interactuable>()) {
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().constraints= RigidbodyConstraints.None;
            this.GetComponentInChildren<Light>().range = 12;

        }
    }
}
