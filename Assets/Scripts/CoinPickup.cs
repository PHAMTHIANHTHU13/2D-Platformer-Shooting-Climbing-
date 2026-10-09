using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip audioEffect;

    bool wasCollected = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        // other là người chơi
        if(other.CompareTag("Player") && !wasCollected) // và nhặt được coin
        {
            wasCollected = true;
            AudioSource.PlayClipAtPoint(audioEffect, transform.position);
            Destroy(gameObject); // nếu chấm collider sẽ chỉ cái obj player
        }
    }
}
