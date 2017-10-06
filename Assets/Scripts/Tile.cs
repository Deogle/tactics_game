using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public GameObject unit;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        unit = other.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("unit"))
        unit = null;
    }
}
