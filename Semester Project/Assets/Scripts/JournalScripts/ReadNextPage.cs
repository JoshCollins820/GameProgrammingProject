using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadNextPage : MonoBehaviour
{
    GameObject JournalUI;

    // Start is called before the first frame update
    void Start()
    {
        JournalUI = GameObject.Find("JournalUI");
    }

    private void OnMouseDown()
    {
        JournalUI.GetComponent<ReadJournal>().NextJournalEntry();
    }
}
