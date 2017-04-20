using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_fall : MonoBehaviour {

    public GameObject groundpound;
    public GameObject explosion;
    public GameObject explosionDust;

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Instantiate(explosion, transform.position, explosion.transform.rotation);
            Instantiate(explosionDust, transform.position, explosionDust.transform.rotation);
        }
	}
    void OnCollisionEnter (Collision col)
    {
        Instantiate(groundpound, transform.position, groundpound.transform.rotation);
    }
}
