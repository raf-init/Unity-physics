using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube_Script : MonoBehaviour
{
    public GameObject go;
    public GameObject ss;
    public GameObject magnet1;
    //Current and next position and velocity on x, y and z axis
    private Vector3 v_next_Vector;
    private Vector3 v_Vector;
    private Vector3 x_next_Vector;
    private Vector3 x_Vector;
    //Gravity & linear drag coefficients
    private float kdrag = 0.1f;
    private float g = 10f;
    private Vector3 a;
    //The magnetic force
    private float magnet_force=5.0f;
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
        InvokeRepeating ("Spawn" , 0.0f, 0.0f);
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async void FixedUpdate() {
        //X-axis
        float distTempx = magnet1.GetComponent<SphereCollider>().transform.position.x - x_Vector.x;
        if (distTempx < distx) {
            distx = distTempx;
            magnet_force = 5.0f;
            //approaching
        } else if (distTempx > distx) { 
            distx = distTempx;
            magnet_force = -5.0f;
            //getting away
        }
        //Y-axis
        float distTempy = magnet1.GetComponent<SphereCollider>().transform.position.y - x_Vector.y;
        if (distTempy < disty) {
            disty = distTempy;
            magnet_force = 5.0f;
            //approaching
        } else if (distTempy > disty) { 
            disty = distTempy;
            magnet_force = -5.0f;
            //getting away
        }
        //Z-axis
        float distTempz = magnet1.GetComponent<SphereCollider>().transform.position.z - x_Vector.z;
        if (distTempz < distz) {
            distz = distTempz;
            magnet_force = 5.0f;
            //approaching
        } else if (distTempz > distz) { 
            distz = distTempz;
            magnet_force = -5.0f;
            //getting away
        }
        //Angles of Fx, Fy and Fz forces after decomposing the magnetic F force
        angleV = magnet1.GetComponent<SphereCollider>().transform.position - x_Vector;
        angleV = go.transform.InverseTransformDirection(angleV);
        anglexy = Mathf.Atan2(angleV.x, angleV.y) * Mathf.Rad2Deg; //from y to x
        anglexz = Mathf.Atan2(angleV.z, angleV.x) * Mathf.Rad2Deg; //from x to z
        anglezy = Mathf.Atan2(angleV.y, angleV.z) * Mathf.Rad2Deg;
        stored_anglexy = Mathf.RoundToInt((float)anglexy);
        stored_anglexz = Mathf.RoundToInt((float)anglexz);
        stored_anglezy = Mathf.RoundToInt((float)anglezy);
        
        // Debug.Log(magnet1_force*Mathf.Cos(stored_anglexy));  
        // Debug.Log(magnet1_force*Mathf.Sin(stored_anglexy)*Mathf.Cos(stored_anglexz));
        // Debug.Log(magnet1_force*Mathf.Sin(stored_anglexy)*Mathf.Sin(stored_anglexz));

        //Acceleration on X-axis
        a.x = (magnet_force*Mathf.Cos(stored_anglexy) -kdrag*v_Vector.x)/go.GetComponent<Rigidbody>().mass;
        //Acceleration on X-axis
        a.y = (-go.GetComponent<Rigidbody>().mass*g + magnet_force*Mathf.Sin(stored_anglexy)*Mathf.Cos(stored_anglexz) - kdrag*v_Vector.y)/go.GetComponent<Rigidbody>().mass;
         //Acceleration on X-axis
        a.z = (magnet_force*Mathf.Sin(stored_anglexy)*Mathf.Sin(stored_anglexz) - kdrag*v_Vector.z)/go.GetComponent<Rigidbody>().mass;
        
        ///////////////////////////////////////// X //////////////////////////////////
            //Calculating the future velocity and then position
            v_next_Vector.x = v_Vector.x + a.x*Time.deltaTime;
            go.GetComponent<Rigidbody>().velocity = v_next_Vector;
            v_Vector.x = go.GetComponent<Rigidbody>().velocity.x;
            x_next_Vector.x = x_Vector.x + v_next_Vector.x*Time.deltaTime;
            go.GetComponent<Rigidbody>().transform.localPosition = x_next_Vector;
            x_Vector.x  =  x_next_Vector.x;

            //Detecting collisions, calculating the future velocity and then position based on the principle of conservation of energy
            if (x_Vector.x > 0.44 && v_Vector.x > 0){ 
                v_next_Vector.x = Mathf.Sqrt(0.1f)*(-v_Vector.x);
                go.GetComponent<Rigidbody>().velocity = v_next_Vector;
                v_Vector.x = go.GetComponent<Rigidbody>().velocity.x;                
                x_next_Vector.x = x_Vector.x + v_next_Vector.x*Time.deltaTime;
                go.GetComponent<Rigidbody>().transform.localPosition = x_next_Vector;
                x_Vector.x = go.GetComponent<Rigidbody>().transform.localPosition.x;
                x_Vector.x  =  x_next_Vector.x;
            }
            if (x_Vector.x < -0.44 && v_Vector.x < 0){    
                v_next_Vector.x = Mathf.Sqrt(0.1f)*(-v_Vector.x);
                go.GetComponent<Rigidbody>().velocity = v_next_Vector;
                v_Vector.x = go.GetComponent<Rigidbody>().velocity.x;
                x_next_Vector.x = x_Vector.x + v_next_Vector.x*Time.deltaTime;
                go.GetComponent<Rigidbody>().transform.localPosition = x_next_Vector;
                x_Vector.x = go.GetComponent<Rigidbody>().transform.localPosition.x;
                x_Vector.x  =  x_next_Vector.x;
            }
            
        ///////////////////////////////////////// Y //////////////////////////////////
        //Calculating the future velocity and then position
        if (x_Vector.y >= -0.45){
            v_next_Vector.y = v_Vector.y + a.y*Time.deltaTime;
            go.GetComponent<Rigidbody>().velocity = v_next_Vector;
            v_Vector.y = go.GetComponent<Rigidbody>().velocity.y;
        
            x_next_Vector.y = x_Vector.y + v_next_Vector.y*Time.deltaTime;
            go.GetComponent<Rigidbody>().transform.localPosition = x_next_Vector;
            x_Vector.y  =  x_next_Vector.y;
        
            //Detecting collisions, calculating the future velocity and then position based on the principle of conservation of energy
            if (x_Vector.y > 0.40 && v_Vector.y > 0){
                v_next_Vector.y = Mathf.Sqrt(0.4f)*(-v_Vector.y);
                go.GetComponent<Rigidbody>().velocity = v_next_Vector;
                v_Vector.y = go.GetComponent<Rigidbody>().velocity.y;                
                x_next_Vector.y = x_Vector.y + v_next_Vector.y*Time.deltaTime;
                go.GetComponent<Rigidbody>().transform.localPosition = x_next_Vector;
                x_Vector.y = go.GetComponent<Rigidbody>().transform.localPosition.y;
                x_Vector.y  =  x_next_Vector.y;
            }
            if (x_Vector.y < -0.40 && v_Vector.y < 0){
                v_next_Vector.y = Mathf.Sqrt(0.4f)*(-v_Vector.y);
                go.GetComponent<Rigidbody>().velocity = v_next_Vector;
                v_Vector.y = go.GetComponent<Rigidbody>().velocity.y;
                
                x_next_Vector.y = x_Vector.y + v_next_Vector.y*Time.deltaTime;
                go.GetComponent<Rigidbody>().transform.localPosition = x_next_Vector;
                x_Vector.y = go.GetComponent<Rigidbody>().transform.localPosition.y;
                x_Vector.y  =  x_next_Vector.y;
            }
        }
        else {
             //do nothing
        }
        ///////////////////////////////////////// Z //////////////////////////////////
        //Calculating the future velocity and then position
        v_next_Vector.z = v_Vector.z + a.z*Time.deltaTime;
        go.GetComponent<Rigidbody>().velocity = v_next_Vector;
        v_Vector.z = go.GetComponent<Rigidbody>().velocity.z;
        x_next_Vector.z = x_Vector.z + v_next_Vector.z*Time.deltaTime;
        go.GetComponent<Rigidbody>().transform.localPosition = x_next_Vector;
        x_Vector.z  =  x_next_Vector.z;

        //Detecting collisions, calculating the future velocity and then position based on the principle of conservation of energy
        if (x_Vector.z > 0.44 && v_Vector.z > 0){    
            v_next_Vector.z = Mathf.Sqrt(0.1f)*(-v_Vector.z);
            go.GetComponent<Rigidbody>().velocity = v_next_Vector;
            v_Vector.z = go.GetComponent<Rigidbody>().velocity.z;    
            x_next_Vector.z = x_Vector.z + v_next_Vector.z*Time.deltaTime;
            go.GetComponent<Rigidbody>().transform.localPosition = x_next_Vector;
            x_Vector.z = go.GetComponent<Rigidbody>().transform.localPosition.z;
            x_Vector.z  =  x_next_Vector.z;
        }
        if (x_Vector.z < -0.44 && v_Vector.z < 0){    
            v_next_Vector.z = Mathf.Sqrt(0.1f)*(-v_Vector.z);
            go.GetComponent<Rigidbody>().velocity = v_next_Vector;
            v_Vector.z = go.GetComponent<Rigidbody>().velocity.z;    
            x_next_Vector.z = x_Vector.z + v_next_Vector.z*Time.deltaTime;
            go.GetComponent<Rigidbody>().transform.localPosition = x_next_Vector;
            x_Vector.z = go.GetComponent<Rigidbody>().transform.localPosition.z;
            x_Vector.z  =  x_next_Vector.z;
            }
    }

     void Spawn(){    
        go = Instantiate(ss, new Vector3(0, 0, 0), Quaternion.identity); 
        go.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        v_Vector = go.GetComponent<Rigidbody>().velocity;
        x_Vector = go.GetComponent<Rigidbody>().transform.localPosition;
        Physics.IgnoreCollision(go.GetComponent<Collider>(), GetComponent<Collider>()); 
        go.transform.parent = GameObject.Find("Cube").transform;
    }
}
