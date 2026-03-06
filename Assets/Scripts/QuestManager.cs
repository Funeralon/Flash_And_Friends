using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables; 

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public static bool isEndScreenOpen = false;

    [Header("Liste des Quêtes")]
    public PhotoQuest[] allQuests;
    private int currentQuestIndex = 0;

    [Header("UI Quêtes (En Jeu)")]
    public TextMeshProUGUI titreText;
    public TextMeshProUGUI descText;
    public TextMeshProUGUI feedbackText;

    [Header("Caméras")]
    public GameObject mainCamera; 
    public GameObject cameraFin;  

    [Header("Cinématique de Fin")]
    public PlayableDirector outroDirector; 
    public GameObject playerObject; 
    public GameObject gameUI; 

[Header("UI Fin de Jeu")]
    public GameObject endGamePanel;
    public TextMeshProUGUI finalScoreText;
    public Button quitButton;
    public Button continueButton;

    [Header("UI Journal de Quêtes (Menu 'J')")]
    public GameObject questLogPanel;
    public TextMeshProUGUI questListText;
    private bool isLogOpen = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (endGamePanel != null) endGamePanel.SetActive(false);
        if (questLogPanel != null) questLogPanel.SetActive(false);

        isEndScreenOpen = false;
        Time.timeScale = 1f;

        if (quitButton != null) quitButton.onClick.AddListener(QuitGame);
        if (continueButton != null) continueButton.onClick.AddListener(ContinueGame);

        UpdateQuestUI();
        ActualiserJournal();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && !isEndScreenOpen)
        {
            isLogOpen = !isLogOpen;
            if (questLogPanel != null) questLogPanel.SetActive(isLogOpen);

            if (isLogOpen) ActualiserJournal();
        }
    }

    public void VerifierQuete(string tagPrisEnPhoto)
    {
        if (currentQuestIndex >= allQuests.Length) return;

        PhotoQuest queteActuelle = allQuests[currentQuestIndex];

        if (tagPrisEnPhoto == queteActuelle.tagCible)
        {
            StartCoroutine(AfficherReussite(queteActuelle));

            PhotoMechanic.totalScore += queteActuelle.pointsRecompense;

            currentQuestIndex++;
            UpdateQuestUI();
            ActualiserJournal();
        }
    }

    void UpdateQuestUI()
    {
        if (currentQuestIndex < allQuests.Length)
        {
            titreText.text = "MISSION : " + allQuests[currentQuestIndex].titreQuete;
            descText.text = allQuests[currentQuestIndex].description;
        }
        else
        {
            titreText.text = "MODE EXPLORATION";
            descText.text = "Prends les meilleures photos !";
            DeclencherFinDeJeu();
        }
    }

    void ActualiserJournal()
    {
        if (questListText == null) return;

        string liste = "<b><size=120%>JOURNAL DES QUÊTES</size></b>\n\n";

        for (int i = 0; i < allQuests.Length; i++)
        {
            if (i < currentQuestIndex)
            {
                liste += "<color=#888888><s>[V] " + allQuests[i].titreQuete + "</s></color>\n\n";
            }
            else if (i == currentQuestIndex)
            {
                liste += "<color=#FFD700><b>[En cours] " + allQuests[i].titreQuete + "</b></color>\n\n";
            }
            else
            {
                liste += "[Bloqué] ???\n\n";
            }
        }

        questListText.text = liste;
    }

    void DeclencherFinDeJeu()
    {
        isEndScreenOpen = true;

        if (questLogPanel != null) questLogPanel.SetActive(false);
        if (gameUI != null) gameUI.SetActive(false);

        if (playerObject != null)
        {
            PlayerController scriptMouvement = playerObject.GetComponent<PlayerController>();
            if (scriptMouvement != null) scriptMouvement.enabled = false;
        }

        if (mainCamera != null) mainCamera.SetActive(false);
        if (cameraFin != null) cameraFin.SetActive(true);

        if (outroDirector != null)
        {
            outroDirector.Play();
        }

        finalScoreText.text = "Score 'Good Vibes' : " + PhotoMechanic.totalScore;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ContinueGame()
    {
        isEndScreenOpen = false;

        if (outroDirector != null)
        {
            outroDirector.Stop();
        }

        if (endGamePanel != null) endGamePanel.SetActive(false);

        if (playerObject != null)
        {
            PlayerController scriptMouvement = playerObject.GetComponent<PlayerController>();
            if (scriptMouvement != null) scriptMouvement.enabled = true;
        }

        if (mainCamera != null) mainCamera.SetActive(true);
        if (cameraFin != null) cameraFin.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    IEnumerator AfficherReussite(PhotoQuest quete)
    {
        feedbackText.text = "🎯 QUÊTE ACCOMPLIE :\n" + quete.titreQuete + "\n(+" + quete.pointsRecompense + " points !)";
        yield return new WaitForSeconds(3f);
        feedbackText.text = "";
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}