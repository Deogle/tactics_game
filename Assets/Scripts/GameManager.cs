using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject mainCam;
    public GameObject unit;
    public Player playerUnit;

    private MapMaker map;

	// Use this for initialization
	void Start () {
        map = GetComponent<MapMaker>();
        map.BoardSetup();
        mainCam.transform.position = map.GetCenter(); //Fix camera on the center of the map
        map.SpawnUnit(unit, 3, 1);
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCol = Physics2D.OverlapPoint(mousePos);

            if (hitCol)
            {
                if(hitCol.gameObject.tag == "Player")
                {
                    playerUnit = hitCol.GetComponent<Player>();
                    Debug.Log("Clicked Player Unit");
                    playerUnit.Select();
                }
                else if (hitCol.gameObject.tag == "Tile")
                {
                    Debug.Log("Clicked Wood Panel!");
                    
                }
            }
        }        
	}
}
