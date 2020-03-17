using UnityEngine;

public class CameraController : MonoBehaviour

{  
    public Transform LookAt;
    public Transform camTransform;

    private Camera cam;
    private float distance = 15.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;

    private const float minAngleY = -80.0f;
    private const float maxAngleY = -35.0f;

    private void Start()
    {
        camTransform = transform;
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            currentX += Input.GetAxis("Horizontal");
            currentY -= Input.GetAxis("Vertical");
        }

        if (Input.GetAxis("Mouse Scrollwheel") > 0)
        {
            if (distance > 10f)
                distance--;
        }

        if (Input.GetAxis("Mouse Scrollwheel") < 0)
        {
            if (distance < 20f)
                distance++;
        }

        currentY = Mathf.Clamp(currentY, minAngleY, maxAngleY);
    }

    private void LateUpdate()
    {

        Vector3 dir = new Vector3(0f, 0f, distance);  // -distance
        Quaternion rotation = Quaternion.Euler(currentY, currentX, distance); // 0f
        camTransform.position = LookAt.position + rotation * dir;
        camTransform.LookAt(LookAt.position);
    }
}
