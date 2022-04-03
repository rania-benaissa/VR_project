using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AddAsteroid : MonoBehaviour
{
    public GameObject myPrefab1;
    public GameObject myPrefab2;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Debug.Log("OBJECT CREATED");//+Random.Range(0, 1000));
            GenerateAS(1);
        }
    }

    void GenerateAS(int num)
    {

        for (int i = 0; i < num; i++)
        {
            if (i % 2 == 0)
            {
                GameObject temp = Instantiate(myPrefab1, new Vector3(Random.Range(-200, 1000),
                                                    Random.Range(0, 1000),
                                                    Random.Range(-500, 500))
                                                , Quaternion.identity);
                temp.transform.localScale = temp.transform.localScale * Random.Range(0.5f, 3);
            }
            else
            {
                GameObject temp = Instantiate(myPrefab2, new Vector3(Random.Range(-200, 1000),
                                                    Random.Range(0, 1000),
                                                    Random.Range(-500, 500))
                                               , Quaternion.identity);
                temp.transform.localScale = temp.transform.localScale * Random.Range(0.5f, 3);
            }
          
        }
    }
}
