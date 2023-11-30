using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicPlayer : MonoBehaviour
{
    GameObject Player;
    GameObject PipeInteraction;
    GameObject MusicHorn;
    GameObject Handle;

    AudioSource melody;

    public bool pipeFixed;
    public bool rotating;
    public bool returning;
    public bool interacting;
    public bool leverPulled;
    public float slerpDuration;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Magistrate").transform.GetChild(0).gameObject;
        PipeInteraction = GameObject.Find("PipeInteraction");
        MusicHorn = GameObject.Find("Music Player Horn");
        Handle = transform.GetChild(0).gameObject;
        melody = MusicHorn.GetComponent<AudioSource>();

        pipeFixed = false;
        interacting = false;
        leverPulled = false;
        rotating = false;
        returning = false;
        slerpDuration = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interacting)
        {
            if (!pipeFixed && !leverPulled)
            {
                Player.GetComponent<PlayerUI>().DisableInteractUI();
                Player.GetComponent<PlayerUI>().DisplayHintUI("There is a rush of air\n" +
                    "from the open pipe...");
                rotating = true;

            }
            if (pipeFixed && !leverPulled)
            {
                rotating = true;
                melody.Play();
            }
        }
        if (rotating)
        {
            StartCoroutine(PullLeverOn());
        }
        if (leverPulled)
        {
            Invoke(nameof(ReturnLever), 1.5f);
        }
        if (returning)
        {
            StartCoroutine(PullLeverOff());
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            interacting = true;
            Player.GetComponent<PlayerUI>().DisplayInteractUI("Pull Lever");
            // check if pipe has been fixed;
            pipeFixed = PipeInteraction.GetComponent<FixPipe>().pipeFixed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            interacting = false;
            Player.GetComponent<PlayerUI>().DisableHintUI();
            Player.GetComponent<PlayerUI>().DisableInteractUI();
        }
    }

    void ReturnLever()
    {
        returning = true;
    }

    IEnumerator PullLeverOn()
    {
        float timeElapsed = 0;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(25, 0, 0);
        while (timeElapsed < slerpDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / slerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
        Debug.Log("Cell door opened");
        rotating = false;
        leverPulled = true;
    }

    IEnumerator PullLeverOff()
    {
        float timeElapsed = 0;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(-25, 0, 0);
        while (timeElapsed < slerpDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / slerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
        Debug.Log("Cell door opened");
        returning = false;
        leverPulled = false;
    }


}
