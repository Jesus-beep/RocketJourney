using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidShower : MonoBehaviour
{
    //Erstellt: Elliot am 10.08.2021

    private Vector3 startingPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] [Range(0f, 1f)] private float Factor;
    private float time;
    public float speed = 1f;
    private float timePassed;
    private float endX;
    public Vector3 newStart;
    public GameObject rocket;
    private Vector3 moveStart;
    private float rotateX;
    private float rotateY;
    private float rotateZ;

    // Start is called before the first frame update
    void Start()
    {
        rotateX = Random.Range(-0.3f,0.3f);
        rotateY = Random.Range(-0.3f,0.3f);
        rotateZ = Random.Range(-0.3f,0.3f);
        endX = Random.Range(-60f,60f);
        time = Time.time;
        startingPosition = new Vector3(Random.Range(-10f,10f), transform.position.y, transform.position.z);
        moveStart = startingPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //rotiert den Asteoriden
        transform.Rotate(rotateX, rotateY, rotateZ, Space.World);
        if (timePassed > 10f && Vector3.Distance(rocket.transform.position, transform.position) > 25f) {
            time = Time.time;
            timePassed = 0f;
            newStart.x = startingPosition.x + rocket.transform.position.x + Random.Range(-10f,10f);
            newStart.y = startingPosition.y + rocket.transform.position.y + 20f;
            transform.position = newStart;
            endX = rocket.transform.position.x + Random.Range(-60f,60f);
            moveStart = newStart;
        }
        timePassed = Time.time - time;
        Factor = timePassed * speed;
        transform.position = Vector3.MoveTowards(moveStart, new Vector3(transform.position.x, transform.position.y-20, transform.position.z), Factor);
    }
}
