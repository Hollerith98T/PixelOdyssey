using UnityEngine;
using UnityEngine.UI;

public class CreditsSystem : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private RectTransform crediti;
    [SerializeField] private GameObject startButton;
    [SerializeField] private float scrollSpeed = 30f;

    private bool isScrolling = false;
    private float initialY;
    private Text creditsText;

    private string testoCrediti = @"
CREDITI

Una produzione
MAZZARA INTERACTIVE

Sviluppato da
Giuseppe Mazzara

Direzione artistica
Keanu Reeves
Quentin Tarantino

Musiche originali
Hans Zimmer
Lady Gaga

Sceneggiatura
Christopher Nolan
Elon Musk

Testi e dialoghi
Morgan Freeman

Design UI UX
Tim Burton
Sofia Coppola

Quality Assurance
David Attenborough
Oprah Winfrey

Effetti sonori
Rami Malek
Billie Eilish

Supporto tecnico
Gordon Ramsay

Ringraziamenti speciali a
Mr Bean per l'ispirazione creativa
Dwayne The Rock Johnson per la motivazione
Le 527 tazze di caffè americano sorseggiate

Tecnologie utilizzate
Unity Engine
Un pizzico di follia internazionale

Unico nome reale: Giuseppe Mazzara

© 2025 Mazzara Interactive – Tutti i diritti riservati
";

    void Start()
    {
        if (crediti != null)
        {
            initialY = crediti.anchoredPosition.y;
            creditsText = crediti.GetComponent<Text>();
            if (creditsText != null)
                creditsText.text = testoCrediti;
        }
    }

    void Update()
    {
        if (isScrolling && crediti != null)
        {
            crediti.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
        }
    }

    public void EnableCredits()
    {
        if (creditsPanel != null)
            creditsPanel.SetActive(true);

        if (crediti != null)
        {
            crediti.gameObject.SetActive(true);
            crediti.anchoredPosition = new Vector2(crediti.anchoredPosition.x, initialY);
        }

        if (startButton != null)
            startButton.SetActive(true);

        isScrolling = true;
    }

    // Disable the credits panel
    public void DisableCredits()
    {
        if (creditsPanel != null)
            creditsPanel.SetActive(false);

        if (crediti != null)
            crediti.gameObject.SetActive(false);

        if (startButton != null)
            startButton.SetActive(false);

        isScrolling = false;
    }
}
