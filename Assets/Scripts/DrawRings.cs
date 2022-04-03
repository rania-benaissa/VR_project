using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DrawRings : MonoBehaviour
{
    GameObject[] planets;

    LineRenderer[] lines;

    float radius;

    int lineLength = 300;

    float currentDistance = 0;
    float maxDistance = 6500f;

    float angle;

    float maxWidth = 45f;

    float width = 10f;

    public Material material;

    private void Start()
    {

        // the planets i got (except the sun)
        planets = GameObject.FindGameObjectsWithTag("Planet");

        //i put my lines here
        lines = new LineRenderer[planets.Length];

        for (int i = 0; i < planets.Length; i++)
        {

            radius = Vector3.Distance(transform.position, planets[i].transform.position);

            //create my line
            lines[i] = createLineRenderer();

            drawCircle(lines[i], radius);

        }
    }

    private void LateUpdate()
    {
        // si y a mouvement de la cam
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0 || Input.mouseScrollDelta.y > 0 || Input.mouseScrollDelta.y < 0)
        {
            // THIS IS THE CODE
            /* currentDistance = Vector3.Distance(Camera.main.transform.position, transform.position);
             width = (currentDistance * maxWidth) / maxDistance;*/

            for (int i = 0; i < planets.Length; i++)
            {

                //la distance entre la caméra et les planetes
                currentDistance = Vector3.Distance(Camera.main.transform.position, planets[i].transform.position);

                width = (currentDistance * maxWidth) / maxDistance;

                //ebug.Log(width);

                lines[i].startWidth = width;
                lines[i].endWidth = width;

            }

        }

    }

    LineRenderer createLineRenderer()
    {
        //if needed i can keep a the objects -> for the moment aint necessary
        LineRenderer line = new GameObject().AddComponent<LineRenderer>();

        // largeur du trait
        line.startWidth = width;
        line.endWidth = width;

        //give it a material
        line.material = material;

        // number of verticies in a line
        line.positionCount = (lineLength + 1);

        line.useWorldSpace = false;

        // give the new object the same position + rotation as the sun
        line.gameObject.transform.position = transform.position;
        line.gameObject.transform.eulerAngles = transform.eulerAngles;

        return line;

    }

    void drawCircle(LineRenderer line, float radius)
    {

        // dessin du cercle 
        for (int i = 0; i < lineLength + 1; i++)
        {
            angle = Mathf.Deg2Rad * (i * 360f / lineLength);

            float x = radius * Mathf.Cos(angle);
            float z = radius * Mathf.Sin(angle);

            // on cree le point avec les coordonnées adequats

            line.SetPosition(i, new Vector3(x, 0, z));

        }

    }
}
