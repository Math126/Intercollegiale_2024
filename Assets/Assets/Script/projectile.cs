using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class projectile : MonoBehaviour
{
    public float distanceToDistract = 25;

    private void OnCollisionEnter(Collision collision)
    {
        foreach (GameObject ennemi in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float DistanceWithEnemy = Vector3.Distance(ennemi.transform.position, transform.position);
            if (DistanceWithEnemy <= distanceToDistract)
            {
                if(!ennemi.GetComponent<FieldOfView>().canSeePlayer)
                    ennemi.GetComponent<NavMeshAgent>().SetDestination(transform.position);
                Destroy(collision.transform.GetComponent<projectile>());
            }
        }
    }
}