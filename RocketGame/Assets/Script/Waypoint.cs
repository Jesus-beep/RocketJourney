using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Elliot; 22.07.
public class Waypoint : MonoBehaviour
{
    public Image img;
    public Transform Home;
    public Text distanceToHome;

    private void Update()
    {
        float minX = img.GetPixelAdjustedRect().width - 20;
        float maxX = Screen.width - minX;

        float minY = img.GetPixelAdjustedRect().height + 50;
        float maxY = Screen.height - minY;

        Vector2 pos = Camera.main.WorldToScreenPoint(Home.position);

        if(Vector3.Dot((Home.position - transform.position), transform.forward) < 0)
        {
            if(pos.x < Screen.width / 2)
            {
                pos.x = maxX;
            } else {
                pos.x = minX;
            }
            if(pos.y < Screen.width / 2)
            {
                pos.y = maxY;
            } else {
                pos.y = minY;
            }
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        img.transform.position = pos;

        Vector3 homeVector = Home.position;
        homeVector.y += 2;  //adjust offset

        int distance = (((int)Vector3.Distance(homeVector, transform.position)) - 14);
        distanceToHome.text = distance.ToString();
    }
}