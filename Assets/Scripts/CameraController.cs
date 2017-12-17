using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour{

    public GameObject cameraObj;

	void Start () {
        int camX = this.gameObject.GetComponent<GameManager>().sizeX /2;
        int camY = this.gameObject.GetComponent<GameManager>().sizeY /2;
        cameraObj.transform.position = new Vector3(camX,camY,-10);
    }
}
