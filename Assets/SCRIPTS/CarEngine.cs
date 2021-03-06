﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarEngine : MonoBehaviour {

    DestructibleObj CarHealth;

    public bool isReverse;
    
    private int Score;
    public Text ScoreText;
    public Text GearTextField;
    public Text SpeedField;
    public Transform GameOverScreen;
    public float gearStepVol = 14f;

    //public Transform Path;

    public GameObject pathNode;

    public float MaxSpeed;
    public float MaxMotorTorque = 300f;
    public float MaxSteerAngle = 45f;
    public float TurnAngle = 15f;
    public float TurnDist = 2f;
    public Vector3 CenterOfMass;

    public WheelCollider WheelFL;
    public WheelCollider WheelFR;
    public WheelCollider WheelRL;
    public WheelCollider WheelRR;

    private float CurSpeed;
    private float pitch = 0;
    private bool IsBraking = false;
    public float MaxBrakeTorque = 1000000f;
    private float CurrLine;
    
    private bool Manevr;
    private List<Transform> Nodes;
    private int CurrentNode = 0;
    public int CounterGear = 0;

    [Header("LinesX")]
    private float Line0 = 46.5f;
    private float Line1 = 50;
    private float Line2 = 52.5f;
    private float Line3 = 55f;
    private float Line4 = 57.5f;
    private float Line5 = 60f;
    private float Line6 = 62.5f;
    private float Line7 = 65f;
    private float Line8 = 67.5f;
    private float Line9 = 71f;

   private float linesY = -29f; 

    void Start () {

        GameObject pathNode = new GameObject();
        pathNode.transform.position = this.transform.position;

        GetComponent<Rigidbody>().centerOfMass = CenterOfMass;
        //Transform[] PathTransforms = Path.GetComponentsInChildren<Transform>();
        CarHealth = GetComponent<DestructibleObj>();
        Score = 0;
        ScoreText.text = "";
        CurrLine = Line1;

        WheelFL.motorTorque = 0;
        WheelFR.motorTorque = 0;

        //Nodes = new List<Transform>();
        //for (int i = 0; i < PathTransforms.Length; i++)
        //    if(PathTransforms[i] != Path.transform)
        //    {
        //        Nodes.Add(PathTransforms[i]);
        //    }
	}
	
	private void FixedUpdate () { 
        ApplySteer();
        Drive();
        CheckWaypointDistance(); //InvokeRep maybe?
        Braking();
        SpeedField.text = (GetComponent<Rigidbody>().velocity.magnitude * 3.6).ToString();
        //CarHealth.TakeDamage(1); //Fuel damage here!!
        pitch = CurSpeed / MaxSpeed;
        if (pitch < 1.5f && pitch > 0.1)
            GetComponent<AudioSource>().pitch = pitch;
    }

    

    public void GearUp()
    {
        if (CounterGear < 4 && CounterGear != -1)
        {
            CounterGear++;
            GearTextField.text = CounterGear.ToString();
            MaxSpeed += gearStepVol; //20

        } else if (CounterGear == -1)
        {
            CounterGear++;
            GearTextField.text = CounterGear.ToString();
            MaxSpeed = 0f;
            isReverse = false;
            WheelFL.motorTorque = 0;
            WheelFR.motorTorque = 0;
        }     
    }

    public void GearDown()
    {
        if (CounterGear > 1 )
        {
            CounterGear--;
            GearTextField.text = CounterGear.ToString();
            MaxSpeed -= gearStepVol;

        } else if (CounterGear == 1)
        {
            CounterGear--;
            GearTextField.text = CounterGear.ToString();
            MaxSpeed = 0f;
            WheelFL.motorTorque = 0;
            WheelFR.motorTorque = 0;

        } else if (CounterGear == 0)
        {
            CounterGear--;
            GearTextField.text = CounterGear.ToString();
            MaxSpeed = gearStepVol;
            isReverse = true;
        }
    }

    public void TurnRight()
    {
        if (CurrLine == Line1)
        {
            pathNode.transform.position = new Vector3(Line2, linesY, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line2;
        }
        else if (CurrLine == Line2)
        {
            pathNode.transform.position = new Vector3(Line3, linesY, gameObject.transform.position.z + TurnAngle); 
            CurrLine = Line3;
        }
        else if (CurrLine == Line3)
        {
            pathNode.transform.position = new Vector3(Line4, linesY, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line4;
        }
        else if (CurrLine == Line4)
        {
            pathNode.transform.position = new Vector3(Line5, linesY, base.gameObject.transform.position.z + TurnAngle);
            CurrLine = Line5;
        }
        else if (CurrLine == Line5)
        {
            pathNode.transform.position = new Vector3(Line6, linesY, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line6;
        }
        else if (CurrLine == Line6)
        {
            pathNode.transform.position = new Vector3(Line7, linesY, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line7;
        }
        else if (CurrLine == Line7)
        {
            pathNode.transform.position = new Vector3(Line8, linesY, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line8;
        }
        else if (CurrLine == Line8)
        {
            pathNode.transform.position = new Vector3(Line9, linesY, gameObject.transform.position.z + TurnAngle*1.5f);
            CurrLine = Line8;
            Manevr = true;
        }

    }

    public void TurnLeft()
    {
        if (CurrLine == Line8)
        {
            pathNode.transform.position = new Vector3(Line7, linesY, gameObject.transform.position.z + TurnAngle); 
            CurrLine = Line7;
        }
        else if (CurrLine == Line7)
        {
            pathNode.transform.position = new Vector3(Line6, linesY, gameObject.transform.position.z + TurnAngle); 
            CurrLine = Line6;
        }
        else if (CurrLine == Line6)
        {
            pathNode.transform.position = new Vector3(Line5, linesY, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line5;
        }
        else if (CurrLine == Line5)
        {
            pathNode.transform.position = new Vector3(Line4, linesY, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line4;
        }
        else if (CurrLine == Line4)
        {
            pathNode.transform.position = new Vector3(Line3, linesY, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line3;
        }
        else if (CurrLine == Line3)
        {
            pathNode.transform.position = new Vector3(Line2, linesY, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line2;
        }
        else if (CurrLine == Line2)
        {
            pathNode.transform.position = new Vector3(Line1, linesY, gameObject.transform.position.z + TurnAngle);
            CurrLine = Line1;
        }
        else if (CurrLine == Line1)
        {
            pathNode.transform.position = new Vector3(Line0, linesY, gameObject.transform.position.z + TurnAngle*1.5f);
            CurrLine = Line1;
            Manevr = true;
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
        if (CarHealth.isDead)
        {
            Score = (((int)transform.position.z)-48)/1000;
            ScoreText.text = ("Stayed on road for  ") + (Score.ToString() + " km");
            GameOverScreen.gameObject.SetActive(true);
            //GetComponent<Rigidbody>().useGravity = false;
            //GetComponent<Rigidbody>().mass = 50;
            //GetComponent<Rigidbody>().AddExplosionForce(1000f, transform.position, 20f);
            //Time.timeScale = 0;
        }
        
        CurSpeed = 2 * Mathf.PI * WheelFL.radius * WheelFL.rpm * 60 / 1000;

        if (isReverse)
        {
            WheelFL.motorTorque = -MaxMotorTorque;
            WheelFR.motorTorque = -MaxMotorTorque;
        }
        else if (CurSpeed < MaxSpeed && !IsBraking && CounterGear != 0)
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

    private void CheckWaypointDistance()
    {
        if (Vector3.Distance(transform.position, pathNode.transform.position) < TurnDist)
        {
            if (Manevr)
            {
                pathNode.transform.position = new Vector3(CurrLine, linesY, pathNode.transform.position.z + TurnAngle * 1.5f);
                Manevr = false;
            } else
            {
                pathNode.transform.position = new Vector3(CurrLine, linesY, pathNode.transform.position.z + TurnAngle);
            }
        }
    }
    
    private void Braking()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsBraking = true;
            WheelRL.brakeTorque = MaxBrakeTorque;
            WheelRR.brakeTorque = MaxBrakeTorque;
           

        } else if(Input.GetKeyUp(KeyCode.Space))
        {
            IsBraking = false;
            WheelRL.brakeTorque = 0;
            WheelRR.brakeTorque = 0;
            
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Collide!");
    //    if (other.gameObject.CompareTag("Enemy"))
    //    {
    //        DestructibleObj enemy = other.transform.GetComponent<DestructibleObj>();
    //        Debug.Log(enemy);
    //        if (enemy != null)
    //        {
    //            enemy.DamageIncome(TaranDmg);
    //        }
    //        //other.gameObject.SetActive(false);
    //        //Score+=1000;
    //        //CarHealth.CurHealth += 1000;
    //    }
    //}
}
