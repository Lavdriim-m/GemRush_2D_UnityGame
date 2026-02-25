using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameController;

public class GameOverMenuController : MonoBehaviour
{

    public TMP_Text survivedText;

    void Start()
    {
        int count = GameData.survivedLevelsCount;
        survivedText.text = "YOU SURVIVED " + count + " LEVEL" + (count != 1 ? "S" : "");
        MusicManager.PauseBackgroundMusic();
        Time.timeScale = 0;
    }

    public void OnRetryClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }
    public void OnMainMenuClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");
    }
}
