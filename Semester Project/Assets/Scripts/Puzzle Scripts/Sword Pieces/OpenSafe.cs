using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSafe : MonoBehaviour
{

    GameObject SwordPiece;
    GameObject Player;
    GameObject Combination;

    float slerpDuration;
    public bool opening;
    public bool interacting;
    public bool showClue;      // true if UI should display clue about safe, false if showing
    public bool showAction;    // true if UI should display action that can be taken on safe
    public bool opened;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Magistrate").transform.GetChild(0).gameObject;
        SwordPiece = GameObject.Find("Piece3");
        Combination = GameObject.Find("Combination");

        slerpDuration = 2f;
        opening = false;
        opened = false;
        showClue = true;
        showAction = true;
        interacting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interacting && !opened)
        {

            Player.GetComponent<PlayerUI>().DisableInteractUI(); // action taken stop display
            if (Combination.GetComponent<PickUpClue>().pickedUp)
            {
                Debug.Log("opening marked");
                opening = true;
            }
            else if (showClue)
            {
                Player.GetComponent<PlayerUI>().DisplayHintUI("Locked, I'm going to\n" +
                    "need the combination...");
                showClue = false;
            }

        }
        if (opening)
        {
            Debug.Log("opening chest");
            SwordPiece.SetActive(true);
            StartCoroutine(Rotate90());


        }
        if (opened) // get camera back to player
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
            if (showAction)
            {
                Player.GetComponent<PlayerUI>().DisplayInteractUI("Open Chest");
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
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, -45, 0);
        while (timeElapsed < slerpDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / slerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
        Debug.Log("Safe opened");
        opening = false;
        opened = true;
    }
}
