using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//script for rotating an object on axis * speed //
public class RotateObject : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Vector3 axis;

    void Update()
    {
        //transform.Rotate(new Vector3(Time.deltaTime * 0, 5, 0));
        transform.Rotate(axis * speed * Time.deltaTime);
    }
}
