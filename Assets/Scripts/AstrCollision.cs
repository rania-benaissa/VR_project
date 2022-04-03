using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstrCollision : MonoBehaviour
{
    bool hasExploded;
    public GameObject ExplosionEffect;
    public GameObject soundSource;
    void Start()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        AudioSource source = GetComponent<AudioSource>();
        Debug.Log(gameObject.name + " was Triggered by " + other.gameObject.name);

        // si l'objet qui avec s'est produit un chauvechement qui du tag planet , destroy asteroid
        if (other.gameObject.tag == "Planet" ||
            other.gameObject.tag == "Sat" ||
            other.gameObject.tag == "Earth" ||
            other.gameObject.tag == "Sun" ||
            other.gameObject.tag == "BlckH" ||
            other.gameObject.tag == "Astr")
        {
            // attendre jusqu'a la fin de sound et anime pour destroy
            if (other.gameObject.tag == "Astr")
                source.Stop();
            source.Play();
            //
            GameObject explosion=null;
            if (!hasExploded)
            {
                explosion = Instantiate(ExplosionEffect, transform.position, transform.rotation);
                explosion.transform.localScale = gameObject.transform.localScale*1.2f;
                Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                rb.AddExplosionForce(300f, transform.position, 100f);
                hasExploded = true;
                gameObject.transform.localScale = gameObject.transform.localScale* 0;
            }

            StartCoroutine(ExampleCoroutine( explosion));
            
        }
    }

    IEnumerator ExampleCoroutine(GameObject explosion)
    {
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(3);
        //gameObject.SetActive(false);
        Destroy(explosion);
        Destroy(gameObject);
    }

/*
    void OnCollisionEnter(Collision collision)
    {
        AudioSource source = soundSource.GetComponent<AudioSource>();
        Debug.Log(gameObject.name + " is collided with " + collision.gameObject.name);

        // si l'objet qui avec s'est produit un chauvechement == au nom donnée , destroy asteroid
        // collision when real true body touch real true collider
        if (collision.gameObject.tag == "Planet" ||
            collision.gameObject.tag == "Sat" ||
            collision.gameObject.tag == "Earth" ||
            collision.gameObject.tag == "Sun" ||
            collision.gameObject.tag == "BlckH" ||
            collision.gameObject.tag == "Astr")
        {
            // attendre jusqu'a la fin de sound et anime pour destroy
            if (collision.gameObject.tag == "Astr")
                source.Stop();
            source.Play();
            StartCoroutine(ExampleCoroutine());
            //StartCoroutine(ExampleCoroutine());
        }

    }*/

}
