using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReadJournal : MonoBehaviour
{
    public GameObject Journal;
    public GameObject LeftPage;
    public GameObject RightPage;
    public GameObject NextPage;
    public GameObject PreviousPage;

    Dictionary<string, List<string>> Entries;
    [SerializeField]
    List<string> Log;

    string leftPageContent;
    string rightPageContent;
    string currentEntry;
    int currentEntryIndex;
    [SerializeField]
    bool reading;
    public int pressedJ;

    // Start is called before the first frame update
    void Start()
    {
        Journal = transform.GetChild(0).gameObject;
        LeftPage = Journal.transform.GetChild(0).gameObject;
        RightPage = Journal.transform.GetChild(1).gameObject;
        NextPage = Journal.transform.GetChild(2).gameObject;
        PreviousPage = Journal.transform.GetChild(3).gameObject;
        Entries = new Dictionary<string, List<string>>();
        Log = new List<string>();
        reading = false;
        leftPageContent = "Magistrate's Journal";
        rightPageContent = "TenthMonth 30th day:\n" +
            "I have been sent in the footsteps of Magistrate Cody\n" +
            "who has disappeared along with several others.\n" +
            "Cody believed the disappearances were connected with Blythe Manor.\n" +
            "I arrive tomorrow...";
        List<string> firstEntry = new List<string>
        {
            leftPageContent,
            rightPageContent
        };
        Log.Add("FirstEntry");
        Entries.Add("FirstEntry", firstEntry);
        currentEntry = "FirstEntry";
        currentEntryIndex = 0;
        pressedJ = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // open journal default
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (!reading)
            {
                Debug.Log("Open Journal");
                OpenJournal(currentEntry);
                pressedJ++;
            }
            else
            {
                Debug.Log("Close Journal");
                CloseJournal();
                pressedJ++;
            }
        }

    }

    // when journal opens show relevant entry
    public void OpenJournal(string entryName)
    {
        Debug.Log("Opening Journal");
        Journal.SetActive(true);
        currentEntry = entryName;
        DisplayJournalEntry(entryName);
        reading = true;
    }

    // called by PickUpClue to add entries to the journal
    public void AddEntry(string entryName, List<string> entryContent)
    {
        currentEntry = entryName;
        //first add to Entries dictionary
        Entries.Add(entryName, entryContent);
        //next add entry name to Log
        Log.Add(entryName);
        
    }

    void DisplayJournalEntry(string entryName)
    {
        leftPageContent = Entries[entryName][0];
        rightPageContent = Entries[entryName][1];
        LeftPage.GetComponent<TextMeshProUGUI>().text = leftPageContent;
        RightPage.GetComponent<TextMeshProUGUI>().text = rightPageContent;
        if (Log.IndexOf(entryName) > 0)
            PreviousPage.SetActive(true);
        else if (Log.IndexOf(entryName) == 0)
            PreviousPage.SetActive(false);
        if (Log.IndexOf(entryName) < Log.Count-1)
            NextPage.SetActive(true);
        else if (Log.IndexOf(entryName) == Log.Count-1)
            NextPage.SetActive(false);
    }

    //called when "Next Page" is clicked
    public void NextJournalEntry()
    {
        currentEntryIndex = Log.IndexOf(currentEntry);
        if (currentEntryIndex < Log.Count)
        {
            currentEntryIndex++;
            currentEntry = Log[currentEntryIndex];
            DisplayJournalEntry(currentEntry);
        }
            
    }

    // called when "Previous Page" is clicked
    public void PreviousJournalEntry()
    {
        currentEntryIndex = Log.IndexOf(currentEntry);
        if (currentEntryIndex > 0)
        {
            currentEntryIndex--;
            currentEntry = Log[currentEntryIndex];
            DisplayJournalEntry(currentEntry);
        }
    }

    void CloseJournal()
    {
        Debug.Log("Closing Journal");
        reading = false;
        Journal.SetActive(false);
    }
}
