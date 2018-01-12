using UnityEngine;

public class SkidMarks : MonoBehaviour {

    float CurFriction = 0;
    float SkidAt = 0.4f;
    float MarkWidth = 0.07f;
    int skidding;
    float soundEmission = 2;
    float soundWait = 30;
    GameObject skidSound;

    Vector3[] lastPosition = new Vector3[2];
    public Material SkidMaterial;
		
	void Update () {

        PlayerSkidSound();
	}

    void PlayerSkidSound()
    {
        WheelHit hit;
        WheelCollider collider;
        collider = GetComponent<WheelCollider>();
        collider.GetGroundHit(out hit);
        CurFriction = Mathf.Abs(hit.sidewaysSlip);
        if (SkidAt <= CurFriction && soundWait <= 0)
        {
            skidSound = Instantiate(Resources.Load("SoundSlip"), hit.point, Quaternion.identity) as GameObject;
            soundWait = 1;
        }

        soundWait -= Time.deltaTime * soundEmission;

        if (SkidAt <= CurFriction)
            SkidMesh();
        else skidding = 0;

    }

    void SkidMesh()
    {
        WheelHit hit;
        GetComponent<WheelCollider>().GetGroundHit(out hit);

        GameObject marks = new GameObject("Marks");

        marks.AddComponent<MeshFilter>();
        marks.AddComponent<MeshRenderer>();

        //marks generate
        Mesh MarkMesh = new Mesh();

        //vertices for cube
        Vector3[] vertices = new Vector3[4];
        int[] triangle = new int[6] { 0, 1, 2, 2, 3, 0 };

        if(skidding == 0)
        {
            vertices[0] = hit.point + Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z) * new Vector3(MarkWidth, 0.1f, 0f);
            vertices[1] = hit.point + Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z) * new Vector3(MarkWidth, 0.1f, 0f);
            vertices[2] = hit.point + Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z) * new Vector3(MarkWidth, 0.1f, 0f);
            vertices[3] = hit.point + Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z) * new Vector3(MarkWidth, 0.1f, 0f);
            
            lastPosition[0] = vertices[2];
            lastPosition[1] = vertices[3];
            skidding = 1;
        }
        else
        {
            vertices[0] = lastPosition[1];
            vertices[1] = lastPosition[0];

            vertices[2] = hit.point + Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z) *  new Vector3(-MarkWidth, 0.1f, 0f);
            vertices[3] = hit.point + Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z) * new Vector3(MarkWidth, 0.1f, 0f);

            lastPosition[0] = vertices[2];
            lastPosition[1] = vertices[3];
        }

        MarkMesh.vertices = vertices;
        MarkMesh.triangles = triangle;
        MarkMesh.RecalculateNormals();

        marks.GetComponent<MeshFilter>().mesh = MarkMesh;
        marks.GetComponent<MeshRenderer>().material = SkidMaterial;

        marks.AddComponent<DestroySkidMarks>();
    }

}
