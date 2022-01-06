using System.Collections;
using UnityEngine;

public class ApplyRoad : MonoBehaviour {
    public GameObject LeftTrail;
    public GameObject RightTrail;
    public GameObject Plane;

    private float distance = 0f;

    void RenderColor() {
        LeftTrail.GetComponent<MeshRenderer>().material.color = Color.red;
        RightTrail.GetComponent<MeshRenderer>().material.color = Color.red;
        // Plane.GetComponent<MeshRenderer>().material.color = Color.grey;
    }

    void Move() {
        Plane.transform.position = new Vector3( Plane.transform.position.x, 
                                                Plane.transform.position.y,
                                                Plane.transform.position.z - 1.5f);
        distance += 1.5f;
        // if (distance == 258f) {
        //     print("reset");
        //     distance = 0f;
        //     Plane.transform.position = new Vector3( Plane.transform.position.x, 
        //                                         Plane.transform.position.y,
        //                                         Plane.transform.position.z + 258f);
        // }
        
    }

    void Start() {
        LeftTrail = GameObject.Find("LeftTrail");
        RightTrail = GameObject.Find("RightTrail");
        Plane = GameObject.Find("Plane");

        RenderColor();
    }

    void Update() {
        Move();
    }
}