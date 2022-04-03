using UnityEngine;
using System.Collections;

public class RotateAround : MonoBehaviour
{

    public Transform centerMass;
    public Transform revealingLight;

    public float PlanetRotationDays;
    public float PlanetRotationSpeed = 10.0f;


    public float RotationAroundSunDays = 1.0f;
    public float RotationSpeedAroundSun = 10.0f;

    float defaultEarthYear = 365.25f;
    //this value isnt used now
    //float satelliteOrbitDistance = 1.8f;

    // Use this for initialization
    void Start()
    {

        // For reveal light - must follow Planet and retain the same position
        if (revealingLight != null)
        {
            revealingLight.transform.LookAt(transform.position);
        }

    }

    // Update is called once per frame
    void Update()
    {

        //rotation around sun 
        transform.RotateAround(centerMass.position, Vector3.up, Time.deltaTime * (defaultEarthYear / RotationAroundSunDays) * (RotationSpeedAroundSun) * Time.deltaTime);

        // Rotation around itself
        transform.Rotate(-Vector3.up * Time.deltaTime * PlanetRotationSpeed * PlanetRotationDays);
    }

}
