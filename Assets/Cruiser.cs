using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cruiser : MonoBehaviour
{
    int health;
    float forwardSpeed = 4f;
    public Vector3 destination;
    public bool isMoving = false;
    float shipTurnSpeed = 40f;
    string enemyTAG = "Enemy_Ship";
    Transform target;
    public float rangeOfFire = 50f;

    public Transform partToRotate;
    public Transform partToRotate2;
    public Transform partToRotate3;
    public Transform partToRotate4;

    float turretTurnSpeed = 82f;
    public GameObject shellPrefab;

    private void Start()
    {
        InvokeRepeating("TargetUpdate", 0f, 0.5f);      // starts after 0 sec, interval 0,5s
    }

    void Update()
    {
        ShipMovement();
        if (target == null)
        {
            return;
        }
        //TurnTurrets();

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

    void TargetUpdate()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTAG);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        if (nearestEnemy != null && shortestDistance <= rangeOfFire)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void TurnTurrets()
    {

        Vector3 turnTurretV3 = target.position - transform.position;
        Quaternion turnLookRotation = Quaternion.LookRotation(turnTurretV3);
        Vector3 turretRotation = Quaternion.RotateTowards(partToRotate.rotation, turnLookRotation, Time.deltaTime * turretTurnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, turretRotation.y, 0f);

        Vector3 turretRotation2 = Quaternion.RotateTowards(partToRotate2.rotation, turnLookRotation, Time.deltaTime * turretTurnSpeed).eulerAngles;
        partToRotate2.rotation = Quaternion.Euler(0f, turretRotation2.y, 0f);
        Vector3 turretRotation3 = Quaternion.RotateTowards(partToRotate3.rotation, turnLookRotation, Time.deltaTime * turretTurnSpeed).eulerAngles;
        partToRotate3.rotation = Quaternion.Euler(0f, turretRotation3.y, 0f);
        Vector3 turretRotation4 = Quaternion.RotateTowards(partToRotate4.rotation, turnLookRotation, Time.deltaTime * turretTurnSpeed).eulerAngles;
        partToRotate4.rotation = Quaternion.Euler(0f, turretRotation4.y, 0f);
    }
}
