using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneOnClick : MonoBehaviour
{
    AudioSource note;
    GameObject MusicRoomPuzzle;

    public bool activated;


    // Start is called before the first frame update
    void Start()
    {
        note = this.gameObject.GetComponent<AudioSource>();
        MusicRoomPuzzle = GameObject.Find("Music Room Puzzle");
        activated = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        // play note
        note.Play();

        // add note to puzzle solution
        MusicRoomPuzzle.GetComponent<MusicRoomPuzzle>();

        // let music puzzle script know this has been clicked
        activated = true;
    }

}
