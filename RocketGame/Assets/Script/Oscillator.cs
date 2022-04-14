using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    //Erstellt: Martin am 10.07

    private Vector3 startingPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] [Range(0f, 1f)] private float Factor;
    [SerializeField] float period = 2f;
    private const float tau = Mathf.PI * 2;


    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //zum Meteoriten zwischen zwei Punkte wandern lassen
        if (period <= Mathf.Epsilon) return;

        float cycles = Time.time / period; //growing number

        
        float rawSinWave = Mathf.Sin(cycles * tau); // number between -1 and 1

        Factor = (rawSinWave + 1f) / 2f; //change number to 0 and 1

        transform.position = Vector3.Lerp(startingPosition, new Vector3(endPosition.position.x, endPosition.position.y, endPosition.position.z), Factor);
    }
}
