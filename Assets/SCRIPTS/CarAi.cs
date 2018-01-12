using System.Collections.Generic;
using UnityEngine;

public class CarAi : MonoBehaviour {

    private Transform PlayerTransform;
    public bool avoiding = false; //private
    public enum RoadLines { Line1, Line2, Line3, Line4, Line5, Line6, Line7, Line8 };

    //public Transform TheBus;

    public float chaseRange;
    public float randomX;
    public float randomXlast;
    public float randomZ;
    public float randomZlast;

    public bool isChanging; //private
   // private bool positionFounded;

    public Transform Path;

    public float MaxSpeed = 60f;
    public float MaxMotorTorque = 300f;
    public float MaxSteerAngle = 45f;
    public float TurnAngle = 15f;
    public float CheckDist = 2f;
    public Vector3 CenterOfMass;

    public WheelCollider WheelFL;
    public WheelCollider WheelFR;
    public WheelCollider WheelRL;
    public WheelCollider WheelRR;

    private float CurSpeed;
    private float pitch = 0;
    public bool isBraking = false; //private
    private bool isReverse = false;
    private float MaxBrakeTorque = 10000f;
    public float CurrLine;
    private bool Manevr;

    private List<Transform> Nodes;
    private int CurrentNode = 0;

    [Header("Sensors")]
    public float sensorLength = 10;
    public float fSensorPos = 1;
    public float frSensorPos = 0.5f;
    public float fSensorAngle = 10;

    [Header("Lines")]
    private float Line0 = 46.5f;
    private float Line1 = 50f;
    private float Line2 = 52.5f;
    private float Line3 = 55f;
    private float Line4 = 57.5f;
    private float Line5 = 60f;
    private float Line6 = 62.5f;
    private float Line7 = 65f;
    private float Line8 = 67.5f;
    private float Line9 = 71f;

    void Start () {

        CurrLine = Line3;
        randomXlast = randomX;
        randomZlast = randomZ;

        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        GetComponent<Rigidbody>().centerOfMass = CenterOfMass;
        Transform[] PathTransforms = Path.GetComponentsInChildren<Transform>();
        //CurrLine = Line1;

        Nodes = new List<Transform>();
        for (int i = 0; i < PathTransforms.Length; i++)
            if(PathTransforms[i] != Path.transform)
            {
                Nodes.Add(PathTransforms[i]);
            }
	}

	private void FixedUpdate () {
        if (!avoiding)
            Sensors();
        if (!avoiding)
            LookPosition();
        if (avoiding)
            CheckWaypointDistance();
        ApplySteer();
        Drive();
        Braking();
        //SpeedField.text = (GetComponent<Rigidbody>().velocity.magnitude * 3.6).ToString();
        //CarHealth.TakeDamage(1); //Fuel damage here!!
        pitch = CurSpeed / MaxSpeed;
        if (pitch < 1.1f && pitch > 0.1)
            GetComponent<AudioSource>().pitch = pitch;
    }

    private void Sensors()
    {     
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position;

        sensorStartPos += transform.forward * fSensorPos;
        Vector3 sensorCenterPos = sensorStartPos;

        sensorStartPos.x += frSensorPos;
        Vector3 sensorRightPos = sensorStartPos;

        sensorStartPos.x -= 2 * frSensorPos;
        Vector3 sensorLeftPos = sensorStartPos;

        //avoiding = false;

        //front center
        if (Physics.Raycast(sensorCenterPos, transform.forward, out hit, sensorLength))
        {
            if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Terrain") || hit.collider.CompareTag("Small obstacle") || hit.collider.CompareTag("Player"))
            {
                Debug.DrawLine(sensorStartPos, hit.point);
                avoiding = true;
                TurnRight();
                //isBraking = true;
            }

            ////right
           else if (Physics.Raycast(sensorRightPos, transform.forward, out hit, sensorLength))
            {
                if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Terrain") || hit.collider.CompareTag("Small obstacle") || hit.collider.CompareTag("Player"))
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    avoiding = true;
                    //avoidMulti = 2;
                    TurnLeft();
                }
            }

            ////left
             else if (Physics.Raycast(sensorLeftPos, transform.forward, out hit, sensorLength))
            {
                if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Terrain") || hit.collider.CompareTag("Small obstacle") || hit.collider.CompareTag("Player"))
                {
                    Debug.DrawLine(sensorStartPos, hit.point);
                    avoiding = true;
                    TurnRight();
                }
            }




            ////front right angle
            //if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(fSensorAngle, transform.up) * transform.forward , out hit, sensorLength))
            //{
            //    if (hit.collider.CompareTag("Enemy"))
            //    {
            //        Debug.DrawLine(sensorStartPos, hit.point);
            //        avoiding = true;
            //    }
            //}

            ////front left angle
            //if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-fSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
            //{
            //    if (hit.collider.CompareTag("LightDebris"))
            //    {
            //        Debug.DrawLine(sensorStartPos, hit.point);
            //        avoiding = true;
            //    }
        }
     
    }

    private void LookPosition()
    {
        isBraking = false;
        //if (!avoiding)
        //{
            if (Vector3.Distance(transform.position, PlayerTransform.position) > chaseRange && PlayerTransform.position.z > transform.position.z)
            {
                Nodes[CurrentNode].position = new Vector3(PlayerTransform.position.x, 0.3f, PlayerTransform.position.z - 30);
            }
            else
            {

                Nodes[CurrentNode].position = new Vector3(CurrLine, 0.3f, PlayerTransform.position.z + randomZ);
                if (Nodes[CurrentNode].position.z < transform.position.z && !isChanging)
                {
                    //Nodes[CurrentNode].position = new Vector3((50f) + ((2.5f) * randomX), 0.3f, transform.position.z + TurnAngle);
                    Nodes[CurrentNode].position = new Vector3(CurrLine, 0.3f, transform.position.z + TurnAngle);
                    isBraking = true;
                    if (!isChanging)
                    {
                        Invoke("ChangePos", 5);
                        isChanging = true;
                    }
                }
            }
        //}
    }

    private void ChangePos()
    {
        //randomX = Random.Range(0, System.Enum.GetValues(typeof(RoadLines)).Length);
        randomX = Random.Range(1, 8);
        if (System.Math.Abs(randomX - randomXlast) <= 1)
        {
            if(randomX == 1)
                CurrLine = Line1;
            else if (randomX == 2)
                CurrLine = Line2;
            else if(randomX == 3)
                CurrLine = Line3;
            else if (randomX == 4)
                CurrLine = Line4;
            else if (randomX == 5)
                CurrLine = Line5;
            else if (randomX == 6)
                CurrLine = Line6;
            else if (randomX == 7)
                CurrLine = Line7;
            else if (randomX == 8)
                CurrLine = Line8;

            randomXlast = randomX;
            //CurrLine = (float)RoadLines.Line1;

            randomZ = Random.Range(-20, 20);
            if (randomZ < randomZlast)
            {
                randomZ = randomZlast;
                //isBraking = true;
            }
            else randomZlast = randomZ;
        }
        else randomX = randomXlast;

        isChanging = false;
    }

    private void CheckWaypointDistance()
    {
        if (Vector3.Distance(transform.position, Nodes[CurrentNode].position) < CheckDist)
        {
            Nodes[CurrentNode].position = new Vector3(CurrLine, 0.5f, Nodes[CurrentNode].position.z + TurnAngle);
            avoiding = false;
        }
    }

    private void ApplySteer()
    { 
        Vector3 RelativeVector = transform.InverseTransformPoint(Nodes[CurrentNode].position);
        float NewSteer = (RelativeVector.x / RelativeVector.magnitude) * MaxSteerAngle;
        WheelFL.steerAngle = NewSteer;
        WheelFR.steerAngle = NewSteer;
    }

    private void Drive()
    {
               
        CurSpeed = 2 * Mathf.PI * WheelFL.radius * WheelFL.rpm * 60 / 1000;

        if (isReverse)
        {
            WheelFL.motorTorque = -MaxMotorTorque;
            WheelFR.motorTorque = -MaxMotorTorque;
        }
        else if (CurSpeed < MaxSpeed && !isBraking)
        {
            WheelFL.motorTorque = MaxMotorTorque;
            WheelFR.motorTorque = MaxMotorTorque;
        }
        else
        {
            WheelFL.motorTorque = 0;
            WheelFR.motorTorque = 0;
        }
    }

    private void Braking()
    {
        if (isBraking)
        {
            WheelRL.brakeTorque = MaxBrakeTorque;
            WheelRR.brakeTorque = MaxBrakeTorque;
            //IsBraking = true;
        }
        else 
        {
            WheelRL.brakeTorque = 0;
            WheelRR.brakeTorque = 0;
            //IsBraking = false;
        }
    }

    private  void Reverse() { isReverse = false; }

    public void TurnRight()
    {
        if (CurrLine == Line1)
        {
            Nodes[CurrentNode].position = new Vector3(Line2, 0.5f, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line2;
        }
        else if (CurrLine == Line2)
        {
            Nodes[CurrentNode].position = new Vector3(Line3, 0.5f, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line3;
        }
        else if (CurrLine == Line3)
        {
            Nodes[CurrentNode].position = new Vector3(Line4, 0.5f, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line4;
        }
        else if (CurrLine == Line4)
        {
            Nodes[CurrentNode].position = new Vector3(Line5, 0.5f, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line5;
        }
        else if (CurrLine == Line5)
        {
            Nodes[CurrentNode].position = new Vector3(Line6, 0.5f, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line6;
        }
        else if (CurrLine == Line6)
        {
            Nodes[CurrentNode].position = new Vector3(Line7, 0.5f, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line7;
        }
        else if (CurrLine == Line7)
        {
            Nodes[CurrentNode].position = new Vector3(Line8, 0.5f, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line8;
        }
        else if (CurrLine == Line8)
        {
            Nodes[CurrentNode].position = new Vector3(Line9, 0.5f, gameObject.transform.position.z + TurnAngle * 1.5f);
            CurrLine = Line8;
            Manevr = true;
        }

    }

    public void TurnLeft()
    {
        if (CurrLine == Line8)
        {
            Nodes[CurrentNode].position = new Vector3(Line7, 0.5f, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line7;
        }
        else if (CurrLine == Line7)
        {
            Nodes[CurrentNode].position = new Vector3(Line6, 0.5f, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line6;
        }
        else if (CurrLine == Line6)
        {
            Nodes[CurrentNode].position = new Vector3(Line5, 0.5f, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line5;
        }
        else if (CurrLine == Line5)
        {
            Nodes[CurrentNode].position = new Vector3(Line4, 0.5f, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line4;
        }
        else if (CurrLine == Line4)
        {
            Nodes[CurrentNode].position = new Vector3(Line3, 0.5f,gameObject.transform.position.z + TurnAngle);
            CurrLine = Line3;
        }
        else if (CurrLine == Line3)
        {
            Nodes[CurrentNode].position = new Vector3(Line2, 0.5f, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line2;
        }
        else if (CurrLine == Line2)
        {
            Nodes[CurrentNode].position = new Vector3(Line1, 0.5f, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line1;
        }
        else if (CurrLine == Line1)
        {
            Nodes[CurrentNode].position = new Vector3(Line0, 0.5f, gameObject.transform.position.z + TurnAngle * 1.5f);
            CurrLine = Line1;
            Manevr = true;
        }
    }

}
