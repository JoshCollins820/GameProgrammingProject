using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonDoorOpen : MonoBehaviour
{

    [SerializeField] GameObject DungeonDoorKey;
    [SerializeField] GameObject Player;


    float slerpDuration;
    public bool opening;
    public bool interacting;
    public bool showClue;      // true if UI should display clue about door, false if showing
    public bool showAction;    // true if UI should display action that can be taken on door, false if showing currently
    public bool opened;
    // Start is called before the first frame update
    void Start()
    {
        DungeonDoorKey = GameObject.Find("DungeonDoorKey");
        Player = GameObject.Find("Magistrate").transform.GetChild(0).gameObject;
        slerpDuration = 1f;
        opening = false;
        opened = false;
        showClue = true;
        showAction = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interacting && !opened)
        {

            Player.GetComponent<PlayerUI>().DisableInteractUI(); // action taken stop display
            if (DungeonDoorKey.GetComponent<PickUpClue>().pickedUp)
            {
                Debug.Log("opening marked");
                opening = true;
            }
            else if (showClue)
            {
                Player.GetComponent<PlayerUI>().DisplayHintUI("Locked...");
                showClue = false;
            }
            
        }
        if (opening)
        {
            Debug.Log("opening cell");
            // disable lock
            this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            StartCoroutine(Rotate90());
        }
        if (opened)
        {
            this.gameObject.GetComponent<SphereCollider>().enabled = false;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            interacting = true;
            Debug.Log("Magistrate entered collider");
            if (DungeonDoorKey.GetComponent<PickUpClue>().pickedUp && showAction)
            {
                Player.GetComponent<PlayerUI>().DisplayInteractUI("Use Key on Cell Door");
                showAction = false;
            }
            else if (showAction)
            {
                Debug.Log("Key not picked up yet, display open cell door interact");
                Player.GetComponent<PlayerUI>().DisplayInteractUI("Open Cell Door");
                showAction = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Magistrate exited collider");
            interacting = false;
            showClue = true;
            showAction = true;
            Player.GetComponent<PlayerUI>().DisableHintUI();
            Player.GetComponent<PlayerUI>().DisableInteractUI();
        }
    }
    void OnMouseDown()
    {
        // only open if player has found the key
        if (DungeonDoorKey.GetComponent<PickUpClue>().pickedUp)
        {
            opening = true;
        }
    }

    IEnumerator Rotate90()
    {
        float timeElapsed = 0;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 45, 0);
        while (timeElapsed < slerpDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / slerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
        Debug.Log("Cell door opened");
        opening = false;
        opened = true;
    }
}
