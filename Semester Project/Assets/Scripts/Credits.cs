using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Credits : MonoBehaviour
{
    public bool creditsStarted = false;
    public GameObject blackBackground;
    public GameObject text;
    private float alpha;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(rollCredits), 6);
        blackBackground = GameObject.Find("BlackBackground");
        text = GameObject.Find("CreditText");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void rollCredits()
    {
        creditsStarted = true;
        Invoke(nameof(fadeToBlack), 0.05f);
    }

    private void fadeToBlack()
    {
        var screen_color = blackBackground.GetComponent<Image>().color; // get the current color of blood_screen

        if (creditsStarted == true && blackBackground.GetComponent<Image>().color.a < 1f)
        {
            screen_color.a += 0.02f; // increment opacity by 0.1f
            blackBackground.GetComponent<Image>().color = screen_color;
            Invoke(nameof(fadeToBlack), 0.05f);
        }
        else
        {
            Invoke(nameof(rollUpCredit), 0.02f);
        }
    }

    private void rollUpCredit()
    {
        var credit_location = text.GetComponent<RectTransform>().position;

        if (credit_location.y < 2300)
        {
            credit_location.y += 1f;
            text.GetComponent<RectTransform>().position = credit_location;
            Invoke(nameof(rollUpCredit), 0.02f);
        }
        else
        {
            Application.Quit();
        }
    }
}
