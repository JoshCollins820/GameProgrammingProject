using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneOnClick : MonoBehaviour
{
    //public AudioSource note;
    public GameObject MusicRoomPuzzle;
    public GameObject ActivateSound;

    public bool activated;


    // Start is called before the first frame update
    void Start()
    {
        //note = this.gameObject.GetComponent<AudioSource>();
        MusicRoomPuzzle = GameObject.Find("Music Room Puzzle");
        ActivateSound = transform.GetChild(0).gameObject;
        activated = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateRune()
    {
        if (activated == false)
        {
            Debug.Log("Activated Rune");
            activated = true;
            ActivateSound.SetActive(true);
        }
    }

    public void DeactivateRune()
    {
        if (activated == true)
        {
            Debug.Log("Deactivated Rune");
            activated = false;
            ActivateSound.SetActive(false);
        }
    }
}
