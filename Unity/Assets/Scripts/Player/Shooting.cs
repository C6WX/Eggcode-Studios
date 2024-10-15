using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject bulletPrefab;      
    public Transform firePoint;          
    public float bulletSpeed = 20f;      
    public float fireRate = 0.5f;       
    private float nextFireTime = 0f;
    public float ammoCount = 6;
    public string currentGun = "pistol";
    public float bulletLifetime = 2f;

    void Update()
    {
        // Detect shooting input and check if player can shoot based on fire rate
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        // Calculate the direction from the player to the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - firePoint.position).normalized;

        // Instantiate the bullet at the fire point position
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Get the Rigidbody2D component from the bullet and apply force in the direction of the mouse
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Destroy the bullet after a certain amount of time
        Destroy(bullet, bulletLifetime);
    }
}
