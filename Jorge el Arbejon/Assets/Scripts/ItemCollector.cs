using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private int loot = 0;

    [SerializeField] private Text lootText;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Loot"))
        {
            Destroy(collision.gameObject);

            loot++;

            lootText.text = "Loot: " + loot;
        }
    }
}
