using System.Collections;
using UnityEngine;
using Unity.Services.Analytics;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using System.Collections.Generic;

public class LevelMove_Ref : MonoBehaviour
{
    private bool levelCompleted = false;
    private int levelsPassedInSession = 0;

    private void Start()
    {
        levelsPassedInSession = PlayerPrefs.GetInt("LevelsPassedInSession", 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !levelCompleted)
        {
            levelCompleted = true;
            levelsPassedInSession++;
            PlayerPrefs.SetInt("LevelsPassedInSession", levelsPassedInSession);

            SendLevelStartEvent();
            SendLevelsPassedEvent(); // Ahora usamos RecordEvent() en lugar de CustomEvent()

            GameManager.Instance.NextLevel();
        }
    }

    private async void SendLevelStartEvent()
    {
        try
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
            }

            LevelStartEvent levelStartEvent = new LevelStartEvent
            {
                LevelName = SceneManager.GetActiveScene().name,
                LevelIndex = SceneManager.GetActiveScene().buildIndex
            };

            AnalyticsService.Instance.RecordEvent(levelStartEvent);
            Debug.Log($"LevelStart event recorded successfully for level: {SceneManager.GetActiveScene().name}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error while recording LevelStart event: {e.Message}");
        }
    }

    private async void SendLevelsPassedEvent()
    {
        try
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
            }

            LevelsPassedEvent levelsPassedEvent = new LevelsPassedEvent
            {
                TotalLevelsPassed = levelsPassedInSession 
            };

            AnalyticsService.Instance.RecordEvent(levelsPassedEvent);
            Debug.Log($"LevelsPassedCount event recorded successfully with total_levels_passed: {levelsPassedInSession}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error while sending LevelsPassedCount event: {e.Message}");
        }
    }
}
