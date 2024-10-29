using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrentGun : MonoBehaviour
{
    TMP_Text currentGunText;
    Shooting shootingScript;

    // Start is called before the first frame update
    void Start()
    {
        currentGunText = GetComponent<TMP_Text>();
        shootingScript = GameObject.FindObjectOfType<Shooting>();
    }

    // Update is called once per frame
    void Update()
    {
        currentGunText.text = ("Gun: " + shootingScript.currentGun);
    }
}

