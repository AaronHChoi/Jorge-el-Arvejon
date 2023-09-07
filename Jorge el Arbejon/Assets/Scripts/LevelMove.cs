using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMove_Ref : MonoBehaviour
{

    private bool levelCompleted = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag =="Player" && !levelCompleted)
        {
            levelCompleted = true;
            GameManager.Instance.NextLevel();
        }
    }
}