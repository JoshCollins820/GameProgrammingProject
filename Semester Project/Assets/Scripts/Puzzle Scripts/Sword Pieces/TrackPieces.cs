using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPieces : MonoBehaviour
{
    GameObject JournalUI;

    public int swordPieces;

    public string leftPageContent;
    public string rightPageContent;
    public bool complete;   // found all pieces

    // Start is called before the first frame update
    void Start()
    {
        JournalUI = GameObject.Find("JournalUI");
        swordPieces = 0;
        leftPageContent = "I now have all of the pieces of the sword!\n" +
            "Time to find the forge and complete the\n" +
            "weapon... I think it was in the basement...";
        rightPageContent = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (swordPieces == 4)
        {
            swordPieces++; // stop from repeating
            complete = true;    
            Invoke(nameof(UpdateJournal), 3f);
        }
    }

    public void AddSwordPiece()
    {
        swordPieces++;
    }

    public string GetSwordPieces()
    {
        return swordPieces.ToString();
    }
    void UpdateJournal()
    {
        JournalUI.GetComponent<ReadJournal>().AddEntry(this.name, createEntryContent());
    }

    private List<string> createEntryContent()
    {
        List<string> content = new List<string>
        {
            leftPageContent,
            rightPageContent
        };
        return content;
    }
}
