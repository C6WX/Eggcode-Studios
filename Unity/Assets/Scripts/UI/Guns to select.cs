using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gunstoselect : MonoBehaviour
{
    TMP_Text gunsToSelectText;
    Shooting shootingScript;
    string pistolText = "1 - Pistol";
    string shotgunText = "2 - Shotgun";
    
    // Start is called before the first frame update
    private void Start()
    {
        gunsToSelectText = GetComponent<TMP_Text>();
        shootingScript = GameObject.FindObjectOfType<Shooting>();
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (shootingScript.shotgunUnlocked == false)
        {
            gunsToSelectText.text = pistolText;
        }
        else if (shootingScript.shotgunUnlocked == true)
        {
            gunsToSelectText.text = pistolText + "\n" + shotgunText;
        }
    }
}
