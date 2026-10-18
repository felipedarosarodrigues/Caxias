using System;
using UnityEngine;

public class SetCenterOfMass : MonoBehaviour
{
    public Rigidbody2D rigidbody2D;
    public Vector3 newCenterOfMass;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2D.centerOfMass = newCenterOfMass;
    }

    // Update is called once per frame
    void Update()
    {
        if( MathF.Abs(transform.rotation.z)>0.01f)
        {
            rigidbody2D.AddTorque(transform.rotation.z*Time.deltaTime*30f);
        }
    }
}
