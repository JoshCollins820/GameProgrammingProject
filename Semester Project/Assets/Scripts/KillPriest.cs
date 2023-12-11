using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class KillPriest : MonoBehaviour
{
    GameObject Magistrate;
    GameObject Player;
    GameObject Workshop;
    public GameObject SwordRebuilt;

    bool interacting;
    int KillPriestCutscene;
    // Start is called before the first frame update
    void Start()
    {
        Workshop = GameObject.Find("Workshop");
        SwordRebuilt = Workshop.transform.GetChild(3).gameObject;
        Magistrate = GameObject.Find("Magistrate");
        Player = Magistrate.transform.GetChild(0).gameObject;

        interacting = false;
        KillPriestCutscene = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interacting) 
        {
            interacting = false;
            Player.GetComponent<PlayerUI>().DisableInteractUI();
            SceneManager.LoadScene(KillPriestCutscene);
        }
        //  when player gets sword, Kill trigger becomes active
        if (SwordRebuilt.GetComponent<PickUpClue>().pickedUp)
        {
            GetComponent<SphereCollider>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            interacting = true;
            Player.GetComponent<PlayerUI>().DisplayInteractUI("Kill the Priest");

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            Player.GetComponent<PlayerUI>().DisableInteractUI();
            interacting = false;
        }
    }
}
