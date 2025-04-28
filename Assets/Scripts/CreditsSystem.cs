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
Ludovica Ferrante
Andrea Pagliaro

Musiche originali
Emanuele Corsi
Giorgia Moretti

Sceneggiatura
Sofia Bellini
Marco Ravelli

Testi e dialoghi
Claudio Santini

Design UI/UX
Martina Lodi
Federico Gallo

Quality Assurance
Valerio D’Amico
Elena Bruni

Effetti sonori
Simone Lattanzi
Beatrice Sanna

Supporto tecnico
Riccardo Milani

Ringraziamenti speciali a
Nonna Maria per i panini
Birillo, il cane mascotte del team
Le 136 tazzine di caffè consumate

Tecnologie utilizzate
Unity Engine
Immaginazione e forza di volontà

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

    // Funzione per disattivare il pannello dei crediti
    public void DisableCredits()
    {
        if (creditsPanel != null)
            creditsPanel.SetActive(false);

        if (crediti != null)
            crediti.gameObject.SetActive(false);

        if (startButton != null)
            startButton.SetActive(false);

        isScrolling = false; // Ferma lo scrolling
    }
}
