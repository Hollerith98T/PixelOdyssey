using UnityEngine;

public class Resume : MonoBehaviour
{
    [SerializeField] private GameObject resumeScreen;

    public void ResumeGame()
    {
        CharacterControl.resume = false;
        CharacterControl.moving = true;
        Time.timeScale = 1;

        if (resumeScreen != null)
            resumeScreen.SetActive(false);
    }
}
