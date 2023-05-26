using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int TotalPoints {  get { return totalPoints; } }

    private int totalPoints;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Hay mas de un gamemanager");
        }
    }

    public void AddPoints (int addPoints)
    {
        totalPoints += addPoints;
    }
}
