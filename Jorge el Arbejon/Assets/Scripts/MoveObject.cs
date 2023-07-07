using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveObject : MonoBehaviour
{
    [SerializeField] Transform[] Position;
    [SerializeField] float ObjectSpeed;

    Transform NextPos;

    int NextPosIndex;

    float currentTime = 0;
    public float startingTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = startingTime;

        NextPos = Position[0];
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= 1 * Time.deltaTime;

        if (currentTime <= 0)
        {
            MoveGameObject();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            currentTime = 0;
        }

    }

    void MoveGameObject()
    {

            if (transform.position == NextPos.position)
            {
                NextPosIndex++;
                //if (NextPosIndex >= Position.Length)
                //{
                //    NextPosIndex = 0;
                //}
                NextPos = Position[NextPosIndex];
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, NextPos.position, ObjectSpeed * Time.deltaTime);
            }
    }
}
