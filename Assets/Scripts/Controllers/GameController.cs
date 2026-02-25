using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Cinemachine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    int progressAmount;
    public Slider progressSlider;

    public GameObject player;
    public GameObject loadCanvas;
    public List<GameObject> levels;
    private int currentLevelIndex = 0;

    public CinemachineConfiner2D confiner;
    public List<PolygonCollider2D> levelBounds;

    public List<GameObject> gemGroups; // assign Gems_Level1, Gems_Level2 here

    public Unity.Cinemachine.CinemachineCamera virtualCam;

    public GameObject gameOverScreen;
    public TMP_Text survivedText;
    private int survivedLevelsCount;
    
    public GameObject HoldToLoadInstruction;

    public static class GameData
    {
        public static int survivedLevelsCount;
    }


    public static event Action OnReset;

    public List<GameObject> enemyGroups;

    void Start()
    {
        progressAmount = 0;
        progressSlider.value = 0;
        Gem.OnGemCollect += IncreaseProgressAmount;
        HoldToLoadLevel.OnHoldComplete += LoadNextLevel;
        PlayerHealth.OnPlayedDied += GameOverScreen;
        loadCanvas.SetActive(false);
        for (int i = 0; i < gemGroups.Count; i++)
            gemGroups[i].SetActive(i == currentLevelIndex);
        for (int i = 0; i < enemyGroups.Count; i++)
            enemyGroups[i].SetActive(i == currentLevelIndex);
        gameOverScreen.SetActive(false);
        OnReset += RestoreAllGemsAndEnemies;
    }

    void RestoreAllGemsAndEnemies()
    {
        // Gems:
        foreach (GameObject gemGroup in gemGroups)
        {
            foreach (Transform gem in gemGroup.transform)
                gem.gameObject.SetActive(true);
        }

        // Enemies (we’ll group these next)…
        foreach (GameObject enemyGroup in enemyGroups)
        {
            foreach (Transform enemy in enemyGroup.transform)
                enemy.gameObject.SetActive(true);
        }
    }

    void GameOverScreen()
    {
        //gameOverScreen.SetActive(true);
        //MusicManager.PauseBackgroundMusic();
        //survivedText.text = "YOU SURVIVED " + survivedLevelsCount + " LEVEL";
        //if (survivedLevelsCount != 1) survivedText.text += "S";
        //Time.timeScale = 0;
        GameData.survivedLevelsCount = survivedLevelsCount;
        SceneManager.LoadScene("GameOverScene");
    }

    void OnLevelComplete()
    {
        SceneManager.LoadScene("WinScene");
    }


    public void ResetGame()
    {
        gameOverScreen.SetActive(false);
        MusicManager.PlayBackgroundMusic(true);
        survivedLevelsCount = 0;
        LoadLevel(0, false);
        OnReset.Invoke();
        Time.timeScale = 1;
    }

    void IncreaseProgressAmount(int amount)
    {
        progressAmount += amount;
        progressSlider.value = progressAmount;
        if(progressAmount >= 100)
        {
            if(currentLevelIndex == levels.Count - 1)
            {
                OnLevelComplete();
                ResetGame();
            }
            else
            {
                //Level Complete
                loadCanvas.SetActive(true);
                //Debug.Log("Level Complete");

                if(currentLevelIndex == 0 && HoldToLoadInstruction != null)
                {
                    HoldToLoadInstruction.SetActive(true);
                }
            }
        }
    }

    void LoadLevel(int level, bool wantSurvivedIncrease)
    {
        loadCanvas.SetActive(false);

        levels[currentLevelIndex].gameObject.SetActive(false);
        levels[level].gameObject.SetActive(true);

        gemGroups[currentLevelIndex].SetActive(false);
        gemGroups[level].SetActive(true);

        enemyGroups[currentLevelIndex].SetActive(false);
        enemyGroups[level].SetActive(true);

        player.transform.position = new Vector3(0, 0, 0);

        confiner.BoundingShape2D = levelBounds[level];

        virtualCam.Follow = player.transform;
        virtualCam.ForceCameraPosition(player.transform.position, Quaternion.identity);

        currentLevelIndex = level;
        progressAmount = 0;
        progressSlider.value = 0;
        if (wantSurvivedIncrease) survivedLevelsCount++;
    }

    void LoadNextLevel()
    {
        int nextLevelIndex = (currentLevelIndex == levels.Count - 1) ? 0 : currentLevelIndex + 1;
        LoadLevel(nextLevelIndex, true);
        HoldToLoadInstruction.SetActive(false);
    }

    void Update()
    {
        
    }
}
