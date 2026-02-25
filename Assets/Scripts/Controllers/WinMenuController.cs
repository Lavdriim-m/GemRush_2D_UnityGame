using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenuController : MonoBehaviour
{
    public void OnMainMenuClick()
    {
        MusicManager.PauseBackgroundMusic();
        SceneManager.LoadScene("StartScene");
    }
    
}
