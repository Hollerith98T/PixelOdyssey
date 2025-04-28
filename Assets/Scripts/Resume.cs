using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resume : MonoBehaviour
{
    [SerializeField] public GameObject resumeScreen;

    public void ResumeGame()
    {
        CharacterControl.resume = false;
        CharacterControl.moving = true;
        Time.timeScale = 1;

        resumeScreen.SetActive(false);
    }
}