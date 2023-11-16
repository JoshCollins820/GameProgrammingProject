using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneOnClick : MonoBehaviour
{
    public bool activated;
    public GameObject Glyph;
    GameObject BasementPuzzle;

    // Start is called before the first frame update
    void Start()
    {
        activated = false;
        BasementPuzzle = GameObject.Find("Basement Puzzle");
        Glyph = transform.GetChild(0).GetChild(0).gameObject;

    }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnMouseDown()
    {
        Debug.Log("Stone Clicked");
        if (BasementPuzzle.GetComponent<BasementPuzzle>().puzzleStarted)
        {
            activated = true;
            Glyph.SetActive(true);

        }

    }

    public void resetStone()
    {
        activated = false;
        Glyph.SetActive(false);
    }
}
