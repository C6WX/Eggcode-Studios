using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    LevelSelect levelSelectScript;
    private void Start()
    {
        levelSelectScript = GameObject.Find("1").GetComponent<LevelSelect>().LevelUnlocked;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("hi");
        if (other.gameObject.CompareTag("Player"))
        {
            LoadLevelSelect();
        }
    }
    public void LoadLevelSelect()
    {
        //levelSelectScript.levelUnlocked++;
        levelSelectScript.levelUnlocked++;
        SceneManager.LoadScene("LevelSelect");
    }
}
