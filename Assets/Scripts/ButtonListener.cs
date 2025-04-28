using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListener : MonoBehaviour
{
    [SerializeField] private Text UpText;
    [SerializeField] private Text DownText;
    [SerializeField] private Text RightText;
    [SerializeField] private Text LeftText;

    private bool waitingForKey;
    private KeyCode newKey;

    // Start method to initialize UI texts with current key mappings
    private void Start()
    {
        // Initialize text components with current key mappings
        UpdateTextDisplay();
    }

    // Update text display to show current key mappings
    private void UpdateTextDisplay()
    {
        UpText.text = GameInputManager.GetButton("Up").ToString();
        DownText.text = GameInputManager.GetButton("Down").ToString();
        LeftText.text = GameInputManager.GetButton("Left").ToString();
        RightText.text = GameInputManager.GetButton("Right").ToString();
    }

    // Questi sono i metodi da assegnare ai bottoni
    public void ChangeUpButton()
    {
        StartCoroutine(AssignKey("Up", UpText));
    }

    public void ChangeDownButton()
    {
        StartCoroutine(AssignKey("Down", DownText));
    }

    public void ChangeLeftButton()
    {
        StartCoroutine(AssignKey("Left", LeftText));
    }

    public void ChangeRightButton()
    {
        StartCoroutine(AssignKey("Right", RightText));
    }

    private IEnumerator AssignKey(string keyName, Text textComponent)
    {
        waitingForKey = true;
        textComponent.text = "...";
        newKey = KeyCode.None;

        while (waitingForKey)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(kcode))
                    {
                        newKey = kcode;
                        Debug.Log($"Key pressed: {newKey}");
                        waitingForKey = false;
                        break;
                    }
                }
            }
            yield return null;
        }

        if (newKey != KeyCode.None && newKey != KeyCode.Escape)
        {
            // Apply the new key mapping
            GameInputManager.SetKeyMap(keyName, newKey);
            textComponent.text = newKey.ToString();
            Debug.Log($"Key mapping updated - {keyName}: {newKey}");
        }
        else
        {
            // If ESC was pressed or no valid key was detected, revert to current mapping
            textComponent.text = GameInputManager.GetButton(keyName).ToString();
            Debug.Log($"Key mapping canceled - {keyName} remains: {GameInputManager.GetButton(keyName)}");
        }
    }
}