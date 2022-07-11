using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicController : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float rotationSpeed;

    // We could have this in Update or FixedUpdate, doesn't really matter
    void Update()
    {
        // Move forwards at constant speed
        transform.position += Time.deltaTime * speed * transform.forward;

        // Rotate towards the target
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

}
