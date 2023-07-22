using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int TotalPoints {  get { return totalPoints; } }

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
