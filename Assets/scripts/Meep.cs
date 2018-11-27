using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**Random number generators
     * dont depend on the defaults! 
     * but also dont try to design your own
     * use mersenne twist - psuedorandom number generator
     * mainly used for alternate content  
     **/
public class Meep : MonoBehaviour
{
    public GameObject sphere;
    public GameObject cube;
    public int sphereWeight = 5;
    public int cubeWeight = 15;

    // Use this for initialization
    void Start()
    {

    }

    private void RandomNumberGen()
    {
        int totalWeight = sphereWeight + sphereWeight;
        int roll = Random.Range(1, totalWeight + 1);

        if (roll <= sphereWeight)
        {
           GameObject temp = GameObject.Instantiate(sphere, this.transform);
            Destroy(temp, 1f);
        }
        else
        {
            GameObject temp = GameObject.Instantiate(cube, this.transform);
            Destroy(temp, 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Simplest Case ->50/50 odds
            RandomNumberGen();
        }
    }
}
