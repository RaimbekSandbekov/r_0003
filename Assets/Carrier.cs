using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : MonoBehaviour
{
    int health;
    float forwardSpeed = 3f;
    public Vector3 destination;
    public bool isMoving = false;
    float shipTurnSpeed = 40f;

    void Update()
    {
        ShipMovement();
    }

    void ShipMovement()
    {
        if (isMoving)
        {
            if (Vector3.Distance(transform.position, destination) >= 1f)
            {
                Vector3 dir1 = destination - transform.position;
                transform.Translate(dir1.normalized * forwardSpeed * Time.deltaTime, Space.World);
                Quaternion lookRotation = Quaternion.LookRotation(dir1);
                Vector3 Rotation = Quaternion.RotateTowards(this.transform.rotation, lookRotation, Time.deltaTime * shipTurnSpeed).eulerAngles;
                this.transform.rotation = Quaternion.Euler(0f, Rotation.y, 0f);
            }
            else { isMoving = false; }
        }
    }
}
