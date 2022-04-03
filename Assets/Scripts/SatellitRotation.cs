using UnityEngine;
using System.Collections;

public class SatellitRotation : MonoBehaviour
{

    public Transform centerMass;
    public Transform revealingLight;

    public float SatRotationDays;
    public float SatRotationSpeed = 10.0f;


    public float RotationAroundSunDays = 1.0f;
    public float RotationSpeedAroundSun = 10.0f;

    float defaultEarthYear = 365.25f;
    
    void Start()
    {
        if (revealingLight != null)
        {
            revealingLight.transform.LookAt(transform.position);
        }
    }

    void Update()
    {
        //rotation around sun 
        transform.RotateAround(centerMass.position, Vector3.up,Time.deltaTime * Time.deltaTime * (defaultEarthYear / RotationAroundSunDays) * (RotationSpeedAroundSun));
        transform.Rotate(-Vector3.up * Time.deltaTime * SatRotationSpeed * SatRotationDays);
    }

}
