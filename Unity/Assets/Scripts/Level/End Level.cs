using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    LevelSelect levelSelectScript;
    private void Start()
    {
        //levelSelectScript = GameObject.FindObjectOfType<LevelSelect>();
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LoadLevelSelect();
        }
    }
    
    // Update is called once per frame
    public void LoadLevelSelect()
    {
        //levelSelectScript.levelUnlocked++;
        levelUnlocked++;
        SceneManager.LoadScene("LevelSelect");
    }
}
