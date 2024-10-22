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
            ammoCountText.text = (shootingScript.ammoCount.ToString() + "/" + shootingScript.maxAmmo.ToString());
        }
        else
        {
            ammoCountText.text = ("Reloading...");
        }
        
    }
}
