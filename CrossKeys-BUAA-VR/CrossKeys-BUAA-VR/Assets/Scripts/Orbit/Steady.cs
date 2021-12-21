using System.Collections;
using UnityEngine;

public class Steady : MonoBehaviour {
    public GameObject OrbitLeft;
    public GameObject MonoCamera;
    public GameObject SteadyPoint_Left;

    void resetRotationAndPosition() {
        OrbitLeft.transform.position = new Vector3(SteadyPoint_Left.transform.position.x, SteadyPoint_Left.transform.position.y + (float)0.1, SteadyPoint_Left.transform.position.z - 0.05f);
        OrbitLeft.transform.rotation = MonoCamera.transform.rotation;
        OrbitLeft.transform.Rotate(new Vector3(-115, 0, -90));
    }

    void Start() {
        OrbitLeft = GameObject.Find("Orbit");
        MonoCamera = GameObject.Find("Camera (eye)");
        SteadyPoint_Left = GameObject.Find("SteadyPoint");

        resetRotationAndPosition();
    }

    void Update() {

        resetRotationAndPosition();
    }
}