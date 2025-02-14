using System.Collections;
using UnityEngine;
using Unity.Services.Analytics;
using UnityEngine.SceneManagement;
using Unity.Services.Core;

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

            SendLevelStartEvent(); // Ahora este evento incluye levels_in_session

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
                LevelIndex = SceneManager.GetActiveScene().buildIndex,
                LevelsInSession = levelsPassedInSession // Agregamos la cantidad de niveles pasados en la sesión
            };

            AnalyticsService.Instance.RecordEvent(levelStartEvent);

            Debug.Log($"LevelStart event recorded successfully for level: {SceneManager.GetActiveScene().name} | Levels in session: {levelsPassedInSession}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error while recording LevelStart event: {e.Message}");
        }
    }
}
