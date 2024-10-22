using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    private int levelUnlocked = 1;
    private int levelClickedInt;
    //private string levelClicked;

    //make it so that it gets the buttons name that is clicked and the loads that scene if the levelUnlocked variable is more than or equal to the level number

    // Start is called before the first frame update
    public void Start()
    {
        //gets the name of the object the script is on
        string levelClicked = gameObject.name;
        //changes the levelClicked variable to an int variable
        levelClickedInt = int.Parse(levelClicked);
    }

    public void LevelClicked()
    {
       if (levelClickedInt <= levelUnlocked)
        {
            //loads the level based on levelClickedInt
            SceneManager.LoadScene(levelClickedInt);
        }
    }
}
