using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere_S2_Script : MonoBehaviour
{
    public GameObject go2;
    public GameObject go1;
    public GameObject sphere;
    //Current and next position and velocity on x, y and z axis
    private Vector3 v_next_Vector;
    private Vector3 v_Vector;
    private Vector3 x_next_Vector;
    private Vector3 x_Vector;
    //Gravity, linear drag coefficients, pulling force and acceleration
    private float g = 10f;
    private float kdrag = 0.1f;
    private Vector3 a;
    //Helping variables for calculating the angles for the force decomposition
    private Vector3 angleV;
    private float anglexy;
    private float anglexz;
    private float anglezy;
    private float stored_anglexy;
    private float stored_anglexz;
    private float stored_anglezy;
    public float distx;
    public float disty;
    public float distz;

    // Start is called before the first frame update
    void Start()
    {
        angleV = new Vector3(0,0,0);
        anglexy = 0;
        anglexz = 0;
        anglezy = 0;
        stored_anglexy = 0;
        stored_anglexz = 0;
        stored_anglezy = 0;
        distx = 0;
        disty = 0;
        distz = 0;
        //go2.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        go2.GetComponent<Rigidbody>().velocity = new Vector3(0f, -1f, -3f);
        v_Vector = go2.GetComponent<Rigidbody>().velocity;
        x_Vector = go2.GetComponent<Rigidbody>().transform.localPosition;    
        go1 = GameObject.Find("Particle1");
        Physics.IgnoreCollision(go1.GetComponent<Collider>(), go2.GetComponent<Collider>());       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        //X-axis
        float distobjx = go2.GetComponent<Transform>().localPosition.x - go1.GetComponent<Transform>().localPosition.x;
        if (distobjx < distx){
            distx = distobjx;
            distobjx = -distobjx;
            //approaching
        } else{
            distx = distobjx;
            distobjx = distobjx;
            //getting away
        }
        //Y-axis
        float distobjy = go2.GetComponent<Transform>().localPosition.y - go1.GetComponent<Transform>().localPosition.y;
        if (distobjy < disty){
            disty = distobjy;
            distobjy = -distobjy;
            //approaching
        } else{
            disty = distobjy;
            distobjy = distobjy;
            //getting away
        }
        //Z-axis
        float distobjz = go2.GetComponent<Transform>().localPosition.z - go1.GetComponent<Transform>().localPosition.z;
        if (distobjz < distz){
            distz = distobjz;
            distobjz = -distobjz;
            //approaching
        } else{
            distz = distobjz;
            distobjz = distobjz;
            //getting away
        }

        ////////////////////////////////// ANOTHER APPROACH ///////////////////////////
        // angleV = go1.GetComponent<Transform>().localPosition - x_Vector;
        // angleV = go2.transform.InverseTransformDirection(angleV);
        // anglexy = Mathf.Atan2(angleV.x, angleV.y) * Mathf.Rad2Deg; //apo y pros x
        // anglexz = Mathf.Atan2(angleV.z, angleV.x) * Mathf.Rad2Deg; //apo x pros z
        // anglezy = Mathf.Atan2(angleV.y, angleV.z) * Mathf.Rad2Deg;
        // stored_anglexy = Mathf.RoundToInt((float)anglexy);
        // stored_anglexz = Mathf.RoundToInt((float)anglexz);
        // stored_anglezy = Mathf.RoundToInt((float)anglezy);

        // a.x = (1/(Mathf.RoundToInt((float)distobjx)+0.0001f)*Mathf.Cos(stored_anglexy) -kdrag*v_Vector.x)/go2.GetComponent<Rigidbody>().mass;        
        // a.y = (-go2.GetComponent<Rigidbody>().mass*g + 1/(Mathf.RoundToInt((float)distobjy)+0.0001f)*Mathf.Sin(stored_anglexy)*Mathf.Cos(stored_anglexz) - kdrag*v_Vector.y)/go2.GetComponent<Rigidbody>().mass;
        // a.z = (1/(Mathf.RoundToInt((float)distobjz)+0.0001f)*Mathf.Sin(stored_anglexy)*Mathf.Sin(stored_anglexz) - kdrag*v_Vector.z)/go2.GetComponent<Rigidbody>().mass;
        //Vector3 distobj = go1.GetComponent<Transform>().localPosition - go2.GetComponent<Transform>().localPosition;
        
        //We ignore linear drag and the force of gravity
        //Acceleration on X-axis
        if (distobjx != 0){
            a.x = (1/(distobjx))*0.01f /go2.GetComponent<Rigidbody>().mass;
        }
        //Acceleration on Y-axis
        if (distobjy != 0){
            a.y = (1/(distobjy))*0.01f /go2.GetComponent<Rigidbody>().mass;
        }
        //Acceleration on Z-axis
        if (distobjz != 0){
            a.z = (1/(distobjz))*0.01f /go2.GetComponent<Rigidbody>().mass;
        }
        //Calculating the future velocity and then position
        //Also checking the collision by checking the distance of each particle from the center of the shpere
        Vector3 distTemp = go2.GetComponent<Transform>().localPosition - sphere.GetComponent<Transform>().localPosition;
        if (distTemp.z < 0.4 && distTemp.y < 0.4 && distTemp.x < 0.4 && distTemp.x > -0.4 &&  distTemp.y > -0.4 && distTemp.z > -0.4) { 
            v_next_Vector = v_Vector + a*Time.deltaTime;
            go2.GetComponent<Rigidbody>().velocity = v_next_Vector;
            v_Vector = go2.GetComponent<Rigidbody>().velocity;
            x_next_Vector = x_Vector + v_next_Vector*Time.deltaTime;
            if (x_next_Vector.x < 0.4 && x_next_Vector.x > -0.4 && x_next_Vector.y < 0.4 && x_next_Vector.y > -0.4 && x_next_Vector.z < 0.4 && x_next_Vector.z > -0.4)
            {
                go2.GetComponent<Rigidbody>().transform.localPosition = x_next_Vector;
                x_Vector  =  x_next_Vector;
            }

        } else {
            //We assume that there is no loss of kinetic energy
            v_next_Vector = Mathf.Sqrt(1f)*(-v_Vector);
            go2.GetComponent<Rigidbody>().velocity = v_next_Vector;
            v_Vector = go2.GetComponent<Rigidbody>().velocity;           
            x_next_Vector = x_Vector + v_next_Vector*Time.deltaTime;
            go2.GetComponent<Rigidbody>().transform.localPosition = x_next_Vector;
            x_Vector = go2.GetComponent<Rigidbody>().transform.localPosition;
            x_Vector  =  x_next_Vector;

        }
    }
}
