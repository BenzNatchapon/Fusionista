using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatateToPoint : MonoBehaviour
{
    public Transform target;
    Vector3 targetTemp;

    void Update()
    {
        Vector3 diff = target.position - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 180);

    }
}
