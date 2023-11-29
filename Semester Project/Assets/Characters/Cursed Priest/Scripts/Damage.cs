using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private GameObject player;

    public AudioSource sound;
    public AudioClip swingSound;


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
        sound.PlayOneShot(swingSound, 1.25f);
        player.GetComponent<PlayerStatsTest>().damagePlayer();
    }
}
