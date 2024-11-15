using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject pistolBulletPrefab;
    public GameObject shotgunBulletPrefab;
    public Transform firePoint;
    [SerializeField] private LevelSelect levelSelectScript;
    public float bulletSpeed;
    public float fireRate;
    private float nextFireTime;
    public string currentGun;
    public bool shotgunUnlocked = false;
    public float pistolAmmoCount = 6f;
    public float shotgunAmmoCount = 4f;
    public float bulletLifetime;
    public float reloadTime;
    public int bulletDamage;
    public float maxAmmo = 6;  // Max ammo for pistol (you can adjust for other guns)
    public bool isReloading = false;

    [SerializeField] private Transform ShotGunFirePoint;
    [SerializeField] private Transform PistolFirePoint;
    private Transform SpawnPoint;
    private void Start()
    {
        levelSelectScript = GameObject.Find("1").GetComponent<LevelSelect>();
        currentGun = "Pistol";
        if (LevelSelect.levelUnlocked >= 3)
        {
            shotgunUnlocked = true;
        }
    }

    void Update()
    {
        // Check if the player can shoot (enough ammo, fire rate limit, and not reloading)
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime && !isReloading)
        {
            if (currentGun == "Pistol" && pistolAmmoCount > 0)
            {
                Shoot();
                pistolAmmoCount--;  // Deduct ammo for pistol
            }
            else if (currentGun == "Shotgun" && shotgunAmmoCount > 0)
            {
                Shoot();
                shotgunAmmoCount--;  // Deduct ammo for shotgun
            }

            nextFireTime = Time.time + fireRate;
        }

        // Switch between guns
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentGun = "Pistol";
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && shotgunUnlocked)
        {
            currentGun = "Shotgun";
        }

        // Set weapon stats based on the current gun
        if (currentGun == "Pistol")
        {
            bulletSpeed = 20f;
            fireRate = 0.5f;
            nextFireTime = 0f;
            maxAmmo = 6;
            reloadTime = 2f;
            bulletLifetime = 2f;
            bulletDamage = 10;
        }
        if (currentGun == "Shotgun")
        {
            bulletSpeed = 20f;
            fireRate = 1f;
            nextFireTime = 0.5f;
            maxAmmo = 4;
            reloadTime = 3f;
            bulletLifetime = 1f;
            bulletDamage = 20;
        }

        // Reload script (start reloading if ammo is 0 or player presses 'r')
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && (pistolAmmoCount == 0 || shotgunAmmoCount == 0))
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        // Calculate the direction from the player to the mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - firePoint.position).normalized;

        if (currentGun == "Pistol")
        {
            // Instantiate the pistol bullet at the fire point position
        GameObject bullet = Instantiate(pistolBulletPrefab, PistolFirePoint.position, firePoint.rotation);

            // Get the Rigidbody2D component from the bullet and apply force in the direction of the mouse
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = direction * bulletSpeed;

            // Rotate the fire point to face the mouse direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Destroy the bullet after a certain amount of time
            Destroy(bullet, bulletLifetime);
        }
        else if (currentGun == "Shotgun")
        {   
            // Move the spawn position in front of the player
            // firePoint.up will give us the direction the player is facing
            //Vector3 spawnPosition = firePoint.position + firePoint.up * 0.5f;  // Adjust 0.5f to control how far in front of the player the bullet spawns

            // Ensure the spawn position is in front of the player, accounting for player facing direction
            GameObject bullet = Instantiate(shotgunBulletPrefab, ShotGunFirePoint.position, firePoint.rotation);

            // Get the Rigidbody2D component from the bullet and apply force in the direction of the mouse
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = direction * bulletSpeed;

            // Rotate the fire point to face the mouse direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Destroy the bullet after a certain amount of time
            Destroy(bullet, bulletLifetime);
        }
    }



    IEnumerator Reload()
    {
        isReloading = true; // Set reloading to true to prevent shooting while reloading
        Debug.Log("Reloading...");

        // Wait for the reload time
        yield return new WaitForSeconds(reloadTime);

        // Refill ammo based on the current gun
        if (currentGun == "Pistol")
        {
            pistolAmmoCount = maxAmmo;
        }
        else if (currentGun == "Shotgun")
        {
            shotgunAmmoCount = maxAmmo;
        }

        isReloading = false;
        Debug.Log("Reload complete. Ammo refilled.");
    }
}
