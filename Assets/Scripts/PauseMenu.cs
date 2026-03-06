using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false; 
    public GameObject pauseUI; 

    void Start()
    {
        isPaused = false; 
        pauseUI.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f; 
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }

    void Pause()
    {
        if (AlbumManager.isOpen || QuestManager.isEndScreenOpen) return;

        pauseUI.SetActive(true);
        Time.timeScale = 0f; 
        isPaused = true;
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        isPaused = false;
        SceneManager.LoadScene("Main_Menu");
    }
}