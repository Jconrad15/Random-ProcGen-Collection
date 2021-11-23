using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject floor;

    void FixedUpdate()
    {
        if (floor == null)
        {
            GetFloor();
        }
        else
        {
            transform.LookAt(floor.transform, Vector3.up);
            
            float hMovement = Input.GetAxis("Horizontal");

            transform.RotateAround(floor.transform.position, Vector3.up, -hMovement);
        }
    }

    private void GetFloor()
    {
        floor = GameObject.Find("Floor");
    }
}
