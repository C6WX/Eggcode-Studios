using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static float playerHealth = 100;

    // Update is called once per frame
    void Update()
    {
        //Stops the health from going below 0
        if (playerHealth <= 0)
        {
            playerHealth = 0;
        }
        if (playerHealth <= 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            playerHealth = playerHealth - 30f;
        }
    }
}
