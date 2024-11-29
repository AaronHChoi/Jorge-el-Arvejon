using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Core;

public class PlayerLife : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    [SerializeField] private AudioSource deathSoundEffect;

    private static int deathCount = 0; // Tracks total player deaths

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Ensure Unity Services are initialized
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            UnityServices.InitializeAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Unity Services Initialized Successfully");
                }
                else
                {
                    Debug.LogError("Failed to initialize Unity Services.");
                }
            });
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("trap"))
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("trap"))
        {
            Die();
        }
    }

    private void Die()
    {
        deathSoundEffect.Play();
        anim.SetTrigger("Death");
        rb.bodyType = RigidbodyType2D.Static;

        // Increment the death counter
        deathCount++;

        // Send analytics event
        SendDeathAnalytics();

        // Optionally, restart level after death animation
        Invoke(nameof(LevelRestart), 1.5f); // Delay to allow death animation to play
    }

    private void LevelRestart()
    {
        GameManager.Instance.RestartLevel();
    }

    private void SendDeathAnalytics()
    {
        try
        {
            // Create and configure the analytics event
            HowManyDeathsEvent deathEvent = new HowManyDeathsEvent
            {
                DeathCount = deathCount
            };

            // Log the event before sending
            Debug.Log($"Sending HowManyDeaths event with DeathCount: {deathCount}");

            // Record the event
            AnalyticsService.Instance.RecordEvent(deathEvent);

            // Force event upload
            AnalyticsService.Instance.Flush();
            Debug.Log("HowManyDeaths event flushed successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error while recording HowManyDeaths event: {e.Message}");
        }
    }
}
