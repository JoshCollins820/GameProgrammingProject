using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSittingRoom : MonoBehaviour
{
    GameObject SittingRoomKey;
    GameObject Player;


    float slerpDuration;
    public bool opening;
    public bool interacting;
    public bool showClue;      // true if UI should display clue about door, false if showing
    public bool showAction;    // true if UI should display action that can be taken on door, false if showing currently
    public bool opened;


    // Start is called before the first frame update
    void Start()
    {
        SittingRoomKey = GameObject.Find("SittingRoomKey");
        Player = GameObject.Find("Magistrate").transform.GetChild(0).gameObject;
        slerpDuration = 2f;
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
            if (SittingRoomKey.GetComponent<PickUpClue>().pickedUp)
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
            if (SittingRoomKey.GetComponent<PickUpClue>().pickedUp && showAction)
            {
                Player.GetComponent<PlayerUI>().DisplayInteractUI("Use Key on Sitting Room Door");
                showAction = false;
            }
            else if (showAction)
            {
                Debug.Log("Key not picked up yet, display open cell door interact");
                Player.GetComponent<PlayerUI>().DisplayInteractUI("Open Door");
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

    IEnumerator Rotate90()
    {
        float timeElapsed = 0;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 0, 0);
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
