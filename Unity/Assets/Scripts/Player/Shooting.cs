using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed;
    public float fireRate;
    private float nextFireTime;
    public float ammoCount;
    public string currentGun = "pistol";
    public float bulletLifetime = 2f;
    public float reloadTime;
    public float maxAmmo = 6;  // Max ammo for pistol (you can adjust for other guns)
    public bool isReloading = false;

    void Update()
    {
        // Check if the player can shoot (enough ammo, fire rate limit, and not reloading)
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime && ammoCount > 0 && !isReloading)
        {
            Shoot();
            ammoCount--;
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

        // Rotate the fire point to face the mouse direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Destroy the bullet after a certain amount of time
        Destroy(bullet, bulletLifetime);
    }

    IEnumerator Reload()
    {
        isReloading = true; // Set reloading to true to prevent shooting while reloading
        Debug.Log("Reloading...");

        // Wait for the reload time
        yield return new WaitForSeconds(reloadTime);

        // Refill ammo
        ammoCount = maxAmmo;
        isReloading = false;
        Debug.Log("Reload complete. Ammo refilled.");
    }
}
