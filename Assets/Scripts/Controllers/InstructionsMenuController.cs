using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsMenuController : MonoBehaviour
{
    public void OnPlayClick()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void OnBackClick()
    {
        SceneManager.LoadScene("StartScene");
    }
}
