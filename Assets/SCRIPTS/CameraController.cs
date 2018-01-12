using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject TargetObjectToCatch;

    private Vector3 offset;

    void Start() {offset = transform.position - TargetObjectToCatch.transform.position; }

    void LateUpdate(){ transform.position = TargetObjectToCatch.transform.position + offset; }
}
