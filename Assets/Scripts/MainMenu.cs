using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Main_Festival");
    }

    public void QuitGame()
    {
        Debug.Log("Fermeture du jeu !");
        Application.Quit(); 
    }
}