using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;
using UnityEngine.SceneManagement;
using Unity.Services.Core;

public class StartMenu : MonoBehaviour
{
    public async void StartGame()
    {
        // Send the LevelStart event for the first level
        await SendLevelStartEvent();

        // Load the first level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("StartScreen");
    }

    public void Quit()
    {
        Application.Quit();
    }

    private async System.Threading.Tasks.Task SendLevelStartEvent()
    {
        try
        {
            // Ensure Unity Services are initialized
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
                Debug.Log("Unity Services Initialized Successfully");
            }

            // Create and configure the LevelStartEvent
            LevelStartEvent levelStartEvent = new LevelStartEvent
            {
                LevelName = SceneManager.GetActiveScene().name,
                LevelIndex = SceneManager.GetActiveScene().buildIndex + 1
            };

            // Record the LevelStart event
            AnalyticsService.Instance.RecordEvent(levelStartEvent);

            Debug.Log($"LevelStart event recorded successfully for level: {SceneManager.GetActiveScene().name}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error while recording LevelStart event: {e.Message}");
        }
    }
}
