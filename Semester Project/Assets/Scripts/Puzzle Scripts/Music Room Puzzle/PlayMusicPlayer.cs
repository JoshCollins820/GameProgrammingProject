using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicPlayer : MonoBehaviour
{
    public GameObject Player;
    public GameObject PipeInteraction;
    public GameObject MusicHorn;


    AudioSource melody;

    public bool pipeFixed;
    public bool rotating;
    public bool returning;
    public bool interacting;
    public bool leverPulled;
    public float slerpDuration;
    public Quaternion leverStart;
    public Quaternion leverEnd;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Magistrate").transform.GetChild(0).gameObject;
        PipeInteraction = GameObject.Find("PipeInteraction");
        MusicHorn = GameObject.Find("Music Player Horn");

        melody = MusicHorn.GetComponent<AudioSource>();
        leverStart = transform.rotation;
        leverEnd = transform.rotation * Quaternion.Euler(50, 0, 0);

        pipeFixed = false;
        interacting = false;
        leverPulled = false;
        rotating = false;
        returning = false;
        slerpDuration = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interacting)
        {
            if (!pipeFixed && !leverPulled)
            {
                GameObject.Find("Lever_Sound").GetComponent<AudioSource>().Play();
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
        
        while (timeElapsed < slerpDuration)
        {
            transform.rotation = Quaternion.Slerp(leverStart, leverEnd, timeElapsed / slerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = leverEnd;
        Debug.Log("Lever Pulled");
        rotating = false;
        leverPulled = true;
    }

    IEnumerator PullLeverOff()
    {
        float timeElapsed = 0;

        while (timeElapsed < slerpDuration)
        {
            transform.rotation = Quaternion.Slerp(leverEnd, leverStart, timeElapsed / slerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = leverStart;
        Debug.Log("Lever Returned");
        returning = false;
        leverPulled = false;
    }


}
