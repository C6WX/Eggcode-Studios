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
    public float reloadTime;
    public float maxAmmo = 6;  // Max ammo for pistol (you can adjust for other guns)
    public bool isReloading = false;


    void Update()
    {
        // Detect shooting input and check if player can shoot based on fire rate
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        // Set weapon stats based on the current gun
        if (currentGun == "pistol")
        {
            bulletSpeed = 20f;
            fireRate = 0.5f;
            nextFireTime = 0f;
            maxAmmo = 6;
            reloadTime = 2f;
        }

        // Reload script (start reloading if ammo is 0 or player presses 'r')
        if ((Input.GetKeyDown(KeyCode.R) || ammoCount == 0) && !isReloading)
        {
            StartCoroutine(Reload());
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
    }
}
