using UnityEngine;


public class AsteroidMovement : MonoBehaviour
{

    public GameObject astrd;
    private Rigidbody rb;
    //private float maxSpeed;

    void Start()
    {

        rb = gameObject.GetComponent<Rigidbody>();

        float speedX = Random.Range(500f, 2000f);
        float speedY = Random.Range(500f, 2000f);
        float speedZ = Random.Range(500f, 2000f);

        int selectorX = Random.Range(0, 20);
        int selectorZ = Random.Range(0, 20);
        int selectorY = Random.Range(0, 20);

        //Debug.Log(" Selectorx "+ selectorX);
        //Debug.Log(" SelectorZ " + selectorZ);

        rb.AddForce(transform.forward * speedY * direction(selectorY)); // mouvement sur y
        rb.AddForce(transform.right * speedX * direction(selectorX)); // mouvement sur x
        rb.AddForce(transform.up * speedZ * direction(selectorZ)); // mouvement sur z
    }
    void Update()
    {
        float dynamicMaxSpeed = 1000f;
        rb.velocity = new Vector3( Mathf.Clamp(rb.velocity.x, -dynamicMaxSpeed, dynamicMaxSpeed),
                                   Mathf.Clamp(rb.velocity.y, -dynamicMaxSpeed, dynamicMaxSpeed),
                                   Mathf.Clamp(rb.velocity.z, -dynamicMaxSpeed, dynamicMaxSpeed));
    }
    int direction(int selector)
    {
        if (selector % 2 == 1) return -1;
        else                   return 1;
    }
}
