using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;
using UnityEngine.SceneManagement;
using Unity.Services.Core;

public class LevelMove_Ref : MonoBehaviour
{
    private bool levelCompleted = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !levelCompleted)
        {
            levelCompleted = true;

            // Send the LevelStart event for the next level
            SendLevelStartEvent();

            // Load the next level
            GameManager.Instance.NextLevel();
        }
    }

    private async void SendLevelStartEvent()
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
                LevelIndex = SceneManager.GetActiveScene().buildIndex
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
