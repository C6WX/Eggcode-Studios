using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRenderer : MonoBehaviour
{
    Shooting shootingScript;
    
    public Sprite[] sprites; 
    private SpriteRenderer spriteRenderer;  
    
    void Start()
    {
        shootingScript = GameObject.FindObjectOfType<Shooting>();
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Set the initial sprite
        spriteRenderer.sprite = sprites[0];
    }

    private void Update()
    {
        ChangeSprite();
    }

    private void ChangeSprite()
    {
        if (shootingScript.currentGun == "Pistol")
        {
            spriteRenderer.sprite = sprites[0];
        }

        if (shootingScript.currentGun == "Shotgun")
        {
            spriteRenderer.sprite = sprites[1];
        }
    }
}
