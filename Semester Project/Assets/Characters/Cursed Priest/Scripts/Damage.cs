using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private GameObject player;
    private AudioSource sound;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        sound = GetComponent<AudioSource>();
    }

    // If the player collides with the enemy's hand hitbox
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player punched!");
        sound.Play();
        player.GetComponent<PlayerStatsTest>().damagePlayer();
    }
}
