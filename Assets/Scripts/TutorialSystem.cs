using UnityEngine;

public class TutorialSystem : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject image1;
    [SerializeField] private GameObject image2;
    [SerializeField] private GameObject image3;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject backToMenuButton;
    [SerializeField] private GameObject backButton;

    private int currentImageIndex = 0;

    public void EnableTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(true);

        if (image1 != null)
            image1.SetActive(true);

        if (nextButton != null)
            nextButton.SetActive(true);

        if (backToMenuButton != null)
            backToMenuButton.SetActive(false);

        if (backButton != null)
            backButton.SetActive(false);

        currentImageIndex = 1;
    }

    public void ShowNextImage()
    {
        if (currentImageIndex == 1)
        {
            if (image1 != null)
                image1.SetActive(false);

            if (image2 != null)
                image2.SetActive(true);

            currentImageIndex = 2;
        }
        else if (currentImageIndex == 2)
        {
            if (image2 != null)
                image2.SetActive(false);

            if (image3 != null)
                image3.SetActive(true);

            currentImageIndex = 3;

            if (nextButton != null)
                nextButton.SetActive(false);

            if (backToMenuButton != null)
                backToMenuButton.SetActive(true);

            if (backButton != null)
                backButton.SetActive(true);
        }
    }

    public void DisableTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        if (image1 != null)
            image1.SetActive(false);

        if (image2 != null)
            image2.SetActive(false);

        if (image3 != null)
            image3.SetActive(false);

        if (nextButton != null)
            nextButton.SetActive(false);

        if (backToMenuButton != null)
            backToMenuButton.SetActive(false);

        if (backButton != null)
            backButton.SetActive(false);
    }
}
