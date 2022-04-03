using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;


public class CameraMovement : MonoBehaviour
{

    // horizontal arrows
    float inputH;
    // vertical arrows (up, down)
    float inputV;


    //translation speed
    public float speedT = 20f;

    //to get directionnal light coordinates
    public new Transform light;

    public float mouseSpeed = 10f;

    float xRotation = 0f;

    float yRotation = 0f;

    //UI ELEMENTS
    public GameObject Panel;
    public Text contentText;
    public Text planetNameText;

    //UI STATE
    bool zoom = false;
    int nb_frames=50;

    //saving some infos about the collider 
    Collider planet;

    public ScrollRect scrollRect;


    void Start()

    {
        //initial position of the camera
        transform.position = new Vector3(400, 800, -3000);
        transform.LookAt(GameObject.Find("Sun").transform);
        // to lock the cursor inside the game, "escape" to unlock  
        Cursor.lockState = CursorLockMode.Locked;
        //THE UI IS NOT ACTIVE 
        Panel.gameObject.SetActive(false);
    }


    void Update()
    {
        
        nb_frames++;
        //Borders of the world
        transform.position = new Vector3(
           Mathf.Clamp(transform.position.x, -3000, 3000),
           Mathf.Clamp(transform.position.y, -3000, 3000),
           Mathf.Clamp(transform.position.z, -5000, 5000));

        //right, up ,down, righ
        move();
        scroll();
        if (Input.GetMouseButton(0) && !zoom)    rotate();

        //if we are in the UI
        if (zoom)    ZoomPlanet();
        else Panel.gameObject.SetActive(false); //disable the UI here

    }


    void move()
    {

        // fleches gauche, droite ou bien touches a,d
        inputH = Input.GetAxis("Horizontal");

        // fleches up, down ou bien touches s,w ( on peut changer ofc)
        inputV = Input.GetAxis("Vertical");

        //si on click sur l'une des touches
        if (inputV != 0)
            transform.position += transform.forward * inputV * speedT * (Time.deltaTime * 360);

        if (inputH != 0)
            transform.position += transform.right * inputH * speedT * (Time.deltaTime * 360);

        //give light the same position
        light.position = transform.position;

    }

    void scroll()
    {
        // TO ENABLE SROLLING INSIDE THE UI 
        if(zoom)
        {
            if(scrollRect != null && scrollRect.IsActive() && Input.GetAxis("Mouse ScrollWheel") != 0){
                scrollRect.verticalScrollbar.value += Input.GetAxis("Mouse ScrollWheel") * 50f * Time.smoothDeltaTime;
                //scrollRect.verticalNormalizedPosition += new Vector2(0, Input.GetAxis("Mouse ScrollWheel") * 10f * Time.smoothDeltaTime);
            }
        }
        //ZOOMING ON PLANETS WHEN ON "NAVIGATION MODE"
        else{
            if (Input.mouseScrollDelta.y > 0)
            {
                transform.position += transform.forward * speedT * (Time.deltaTime * 360);
            }
            if (Input.mouseScrollDelta.y < 0)
            {
                transform.position -= transform.forward * speedT * (Time.deltaTime * 360);
            }
        }
    }

    void rotate(){

        Debug.Log(transform.eulerAngles);

        if(nb_frames>50)
        {
            yRotation -= Input.GetAxisRaw("Mouse Y") * mouseSpeed * Time.deltaTime;
            yRotation = Mathf.Clamp(yRotation, -80, 80);
            xRotation += Input.GetAxisRaw("Mouse X") * mouseSpeed * Time.deltaTime;
            xRotation = xRotation % 360;
            transform.localEulerAngles = new Vector3(yRotation,xRotation, 0);      
        }else{

            //force the direction of the camera after exiting the UI
            //transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * mouseSpeed);
            yRotation += Input.GetAxisRaw("Mouse Y") * mouseSpeed * Time.deltaTime;
            xRotation -= Input.GetAxisRaw("Mouse X") * mouseSpeed * Time.deltaTime;
            transform.localEulerAngles = new Vector3(yRotation,xRotation, 0); 

        }
    }




    void OnTriggerEnter(Collider other)
    {
        //if we are colliding but we're not displaying the UI (cuz we can move during the display)
        if ((other.gameObject.tag == "Planet" || other.gameObject.tag == "Sun" || other.gameObject.tag == "Earth") && zoom == false)
        {
            zoom = true;
   
            //saving the collider 
            planet = other;
        
            //position the camera to face the planet 
            transform.position = planet.transform.position + new Vector3(0.0f, other.bounds.size.y, -other.bounds.size.z);
   
            //update & reaveal the UI
            FillUI(other.gameObject.name);
            Panel.gameObject.SetActive(true);

        }

    }


    //gets the infos about planets from the txt files and fills the UI
    void FillUI(string name)
    {
        //looks for the file in the ressources folder
        if (name == "Clouds") name = "Earth";
        object resourceFile = Resources.Load(name);

        // clear content in case of errors
        planetNameText.text = string.Empty;
        contentText.text = string.Empty;

        //IF THE FILE EXISTS
        if (resourceFile != null)
        {
            resourceFile = resourceFile.ToString().Replace("TAB", "\t");
            contentText.text += resourceFile;
            planetNameText.text = name;
        }
        else
        {
            contentText.text = string.Format("Please add the file {0}.txt", name);
            planetNameText.text = string.Format("Error");
        }

    }


    //Focus the cam on the planet 
    void ZoomPlanet(){

        //Camera looking at the planet
        transform.LookAt(planet.transform);

        // to exit UI and go back to navigating freely 
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            zoom = false;
            nb_frames=0;
        }

    }

}