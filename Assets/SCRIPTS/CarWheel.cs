using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWheel : MonoBehaviour {

    public WheelCollider TargetWheel;
    private Vector3 WheelPosition = new Vector3();
    private Quaternion WheelRotation = new Quaternion();
	
	
	private void Update () {

        TargetWheel.GetWorldPose(out WheelPosition, out WheelRotation);
        transform.position = WheelPosition;
        transform.rotation = WheelRotation;

		
	}
}
