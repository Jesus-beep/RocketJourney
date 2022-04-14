using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    //Erstellt: Elliot am 10.08.2021

    public float rotateX;
    public float rotateY;
    public float rotateZ;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateX, rotateY, rotateZ, Space.Self);
    }
}
