using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    public float RotX;
    public float RotY;
    public float RotZ;

    void Update () {
        transform.Rotate(new Vector3(RotX, RotY, RotZ) * Time.deltaTime);
	}
}
