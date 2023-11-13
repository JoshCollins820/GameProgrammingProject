using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonDoorOpen : MonoBehaviour
{

    GameObject DungeonDoorKey;


    float slerpDuration;
    bool opening;

    // Start is called before the first frame update
    void Start()
    {
        DungeonDoorKey = GameObject.Find("DungeonDoorKey");
        opening = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (opening)
        {
            StartCoroutine(Rotate90());
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
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 90, 0);
        while (timeElapsed < slerpDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / slerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
        opening = false;
    }
}
