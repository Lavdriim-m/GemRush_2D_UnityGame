using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseUI; // your in‑scene Canvas

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        bool isPaused = (Time.timeScale == 0);
        Time.timeScale = isPaused ? 1 : 0;
        pauseUI.SetActive(!isPaused);

        if (isPaused)
        {
            MusicManager.PlayBackgroundMusic(true);
        }
        else
        {
            MusicManager.PauseBackgroundMusic();
        }
    }

    public void OnResumeClick()
    {
        TogglePause();
    }

    public void OnMainMenuClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");
    }
}
