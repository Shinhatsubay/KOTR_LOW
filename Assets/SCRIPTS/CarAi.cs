using System.Collections.Generic;
using UnityEngine;

public class CarAi : MonoBehaviour {

    public GameObject pathNode;

    public Transform PlayerTransform;

    public bool avoiding = false;
    public enum RoadLines { Line1, Line2, Line3, Line4, Line5, Line6, Line7, Line8 };

    public float chaseRange;
    public float randomX;
    public float randomXlast;
    public float randomZ;
    public float randomZlast;

    public float checkY = -29f;

    public bool isChanging;

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
    public bool isBraking = false;
    private bool isReverse = false;
    public float MaxBrakeTorque = 100000f;
    public float CurrLine;
    private bool Manevr;

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

        GameObject pathNode = new GameObject(); 
        pathNode.transform.position = PlayerTransform.position;

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
                randomX = Random.Range(1, 2);
                if (randomX > 1)
                    TurnRight();
                else TurnLeft();
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
        }
     
    }

    private void LookPosition()
    {
        isBraking = false;
        //if (!avoiding)
        //{
            if (Vector3.Distance(transform.position, PlayerTransform.position) > chaseRange && PlayerTransform.position.z > transform.position.z)
            {
                pathNode.transform.position = new Vector3(PlayerTransform.position.x, checkY, PlayerTransform.position.z - 40f);
            }
            //else if ((PlayerTransform.position.z + 20f) < transform.position.z)
            //{
            //    isBraking = true;
            //}
            else
            {
            pathNode.transform.position = new Vector3(CurrLine, checkY, PlayerTransform.position.z + randomZ);
                if (pathNode.transform.position.z < transform.position.z && !isChanging)
                {
                    pathNode.transform.position = new Vector3(CurrLine, checkY, transform.position.z + TurnAngle);
                    isBraking = true;
                        if (!isChanging)
                        {
                            Invoke("ChangePos", 10f); //5
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

            randomZ = Random.Range(-10, 10);
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
        if (Vector3.Distance(transform.position, pathNode.transform.position) < CheckDist)
        {
            pathNode.transform.position = new Vector3(CurrLine, checkY, pathNode.transform.position.z + TurnAngle); // y = 0.5f
            avoiding = false;
        }
    }

    private void ApplySteer()
    { 
        Vector3 RelativeVector = transform.InverseTransformPoint(pathNode.transform.position);
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
            pathNode.transform.position = new Vector3(Line2, checkY, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line2;
        }
        else if (CurrLine == Line2)
        {
            pathNode.transform.position = new Vector3(Line3, checkY, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line3;
        }
        else if (CurrLine == Line3)
        {
            pathNode.transform.position = new Vector3(Line4, checkY, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line4;
        }
        else if (CurrLine == Line4)
        {
            pathNode.transform.position = new Vector3(Line5, checkY, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line5;
        }
        else if (CurrLine == Line5)
        {
            pathNode.transform.position = new Vector3(Line6, checkY, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line6;
        }
        else if (CurrLine == Line6)
        {
            pathNode.transform.position = new Vector3(Line7, checkY, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line7;
        }
        else if (CurrLine == Line7)
        {
            pathNode.transform.position = new Vector3(Line8, checkY, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line8;
        }
        else if (CurrLine == Line8)
        {
            pathNode.transform.position = new Vector3(Line9, checkY, gameObject.transform.position.z + (TurnAngle * 1.5f));
            CurrLine = Line8;
            Manevr = true;
        }

    }

    public void TurnLeft()
    {
        if (CurrLine == Line8)
        {
            pathNode.transform.position = new Vector3(Line7, checkY, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line7;
        }
        else if (CurrLine == Line7)
        {
            pathNode.transform.position = new Vector3(Line6, checkY, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line6;
        }
        else if (CurrLine == Line6)
        {
            pathNode.transform.position = new Vector3(Line5, checkY, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line5;
        }
        else if (CurrLine == Line5)
        {
            pathNode.transform.position = new Vector3(Line4, checkY, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line4;
        }
        else if (CurrLine == Line4)
        {
            pathNode.transform.position = new Vector3(Line3, checkY, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line3;
        }
        else if (CurrLine == Line3)
        {
            pathNode.transform.position = new Vector3(Line2, checkY, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line2;
        }
        else if (CurrLine == Line2)
        {
            pathNode.transform.position = new Vector3(Line1, checkY, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line1;
        }
        else if (CurrLine == Line1)
        {
            pathNode.transform.position = new Vector3(Line0, checkY, gameObject.transform.position.z + (TurnAngle * 1.5f));
            CurrLine = Line1;
            Manevr = true;
        }
    }
}
