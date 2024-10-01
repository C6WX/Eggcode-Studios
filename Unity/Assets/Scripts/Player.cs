using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static float playerHealth = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //NEEDS TO BE WORKED ON. HEALTH IS STILL GOING BELOW 0
        //Stops the health from going below 0
        if (playerHealth < 0)
        {
            playerHealth = 0;
        }
    }
}
