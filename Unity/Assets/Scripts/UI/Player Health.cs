using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    TMP_Text playerHealthText;

    // Start is called before the first frame update
    void Start()
    {
        playerHealthText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        playerHealthText.text = ("Health: ") + Player.playerHealth;
    }
}
