using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoCount : MonoBehaviour
{
    TMP_Text ammoCountText;
    Shooting shootingScript;

    // Start is called before the first frame update
    void Start()
    {
        ammoCountText = GetComponent<TMP_Text>();
        shootingScript = GameObject.FindObjectOfType<Shooting>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shootingScript.isReloading == false)
        {
            if (shootingScript.currentGun == "Pistol")
            {
                ammoCountText.text = (shootingScript.pistolAmmoCount.ToString() + "/" + shootingScript.maxAmmo.ToString());
            }
            if (shootingScript.currentGun == "Shotgun")
            {
                ammoCountText.text = (shootingScript.shotgunAmmoCount.ToString() + "/" + shootingScript.maxAmmo.ToString());
            }
        }
        if (shootingScript.isReloading == true)
        {
            ammoCountText.text = ("Reloading...");
        }       
    }
}
