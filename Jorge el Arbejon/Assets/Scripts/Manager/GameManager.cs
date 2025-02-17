using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int TotalPoints {  get { return totalPoints; } }
    private int enemiesKilledInSession = 0;

    private int totalPoints;

    [SerializeField] Animator transitionAnim;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {

            Debug.Log("Hay mas de un gamemanager");

            Destroy(gameObject);
        }
    }

    public void AddPoints (int addPoints)
    {
        totalPoints += addPoints;
    }

    public void NextLevel()
    {
        StartCoroutine(LoadLevel());
    }

    public void RestartLevel()
    {
        StartCoroutine(ResetLevel());
    }
    public void EnemyKilled()
    {
        enemiesKilledInSession++;
        PlayerPrefs.SetInt("EnemiesKilledInSession", enemiesKilledInSession);
        SendEnemiesKilledEvent();
    }

    private async void SendEnemiesKilledEvent()
    {
        try
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
            }

            EnemiesKilledEvent enemiesKilledEvent = new EnemiesKilledEvent
            {
                TotalEnemiesKilled = enemiesKilledInSession
            };

            AnalyticsService.Instance.RecordEvent(enemiesKilledEvent);
            Debug.Log($"EnemiesKilledInSession event recorded successfully: {enemiesKilledInSession}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error while sending EnemiesKilledInSession event: {e.Message}");
        }
    }
    IEnumerator LoadLevel()
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        transitionAnim.SetTrigger("Start");
    }

    IEnumerator ResetLevel()
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        transitionAnim.SetTrigger("Start");
    }

   
}
