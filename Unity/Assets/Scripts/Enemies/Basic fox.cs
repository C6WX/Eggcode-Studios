using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basicfox : MonoBehaviour
{
    public float basicFoxDamage = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //when the fox touches the player, it reduces the player's health
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.playerHealth = Player.playerHealth - basicFoxDamage;
            Debug.Log(Player.playerHealth);
        }
         
    }
}
