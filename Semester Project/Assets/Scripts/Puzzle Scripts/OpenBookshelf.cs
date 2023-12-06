using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBookshelf : MonoBehaviour
{
    [SerializeField] GameObject Player;
    Vector3 closedPosition;
    Vector3 openedPosition;
    [SerializeField] bool interacting;
    [SerializeField] bool opening;
    [SerializeField] bool opened;
    float lerpDuration;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        closedPosition = transform.position;
        openedPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2.2f);
        interacting = false;
        opening = false;
        opened = false;
        lerpDuration = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interacting && !opened)
        {
            Player.GetComponent<PlayerUI>().DisableInteractUI();
            opening = true;
        }
        if (opening)
        {
            StartCoroutine(OpenShelf());
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && !opened)
        {
            Player.GetComponent<PlayerUI>().DisplayInteractUI("Pull Strange Book");
            interacting = true;
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

    IEnumerator OpenShelf()
    {
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            transform.position = Vector3.Lerp(closedPosition, openedPosition, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = openedPosition;
        opening = false;
    }
}
