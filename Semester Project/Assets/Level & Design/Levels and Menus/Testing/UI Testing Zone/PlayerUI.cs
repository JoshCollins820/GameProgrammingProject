using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    // objects
    public GameObject player; // player
    public GameObject stamina_bar; // stamina ui element
    public GameObject blood_screen; // blood screen ui element

    // script local variables
    private float normalized_stamina; // stamina of player, but normalized to 0-1
    private bool blood_cycle_on; // true if the blood screen currently on/cycle is running
    private float blood_cycle_speed; // speed of blood cycle/pulse

    // Start is called before the first frame update
    void Start()
    {
        // initialize values
        player = GameObject.Find("Dummy Player");
        stamina_bar = GameObject.Find("stamina_bar");
        blood_screen = GameObject.Find("blood_screen");
        blood_cycle_on = false;
        blood_cycle_speed = 0.08f;
    }

    // Update is called once per frame
    void Update()
    {
        StaminaBar(); // check for stamina bar updates
        BloodScreen(); // check for blood screen updates
        Debug.Log(blood_screen.GetComponent<Image>().color.a);
    }

    void StaminaBar()
    {
        // normalize player stamina from 0-100 to 0-1
        normalized_stamina = (player.GetComponent<playerStats>().playerStamina) / 100;
        // set bar to player stamina
        stamina_bar.GetComponent<Image>().fillAmount = normalized_stamina;
    }

    // changes blood screen so it is high opacity
    void BloodScreenPulseUp()
    {
        Debug.Log("PulseUp");
        var blood_screen_color = blood_screen.GetComponent<Image>().color; // get the current color of blood_screen

        if (player.GetComponent<playerStats>().playerDamaged == true) // if player is damaged
        {
            if(blood_screen_color.a < 0.7f) // if the blood_screen color's alpha is less than 1f (max)
            {
                blood_screen_color.a += 0.1f; // increment opacity by 0.1f
                blood_screen.GetComponent<Image>().color = blood_screen_color; // update blood_screen color with new modified color
                Invoke(nameof(BloodScreenPulseUp), blood_cycle_speed); // call this function again
            }
            else // if the blood_screen color's alpha hits the 1f (max)
            {
                BloodScreenPulseDown(); // start decreasing opacity
            }
        }
        else // if player is no longer damaged
        { 
            BloodScreenPulseStop(); // stop blood screen effect
        }
    }

    // changes blood screen so it is low opacity
    void BloodScreenPulseDown()
    {
        Debug.Log("PulseDown");
        var blood_screen_color = blood_screen.GetComponent<Image>().color; // get the current color of blood_screen

        if (player.GetComponent<playerStats>().playerDamaged == true) // if player is damaged
        {
            if (blood_screen_color.a > 0.2f) // if the blood_screen color's alpha is greater than 0.5f
            {
                blood_screen_color.a -= 0.1f; // decrease opacity by 0.1f
                blood_screen.GetComponent<Image>().color = blood_screen_color; // update blood_screen color with new modified color
                Invoke(nameof(BloodScreenPulseDown), blood_cycle_speed); // call this function again
            }
            else // if the blood_screen color's alpha hits the 1f (max)
            {
                BloodScreenPulseUp(); // start decreasing opacity
            }
        }
        else // if player is no longer damaged
        {
            BloodScreenPulseStop(); // stop blood screen effect
        }
    }

    // stops blood screen effect by gradually lowering opacity until 0f
    void BloodScreenPulseStop()
    {
        Debug.Log("PulseStop");
        var blood_screen_color = blood_screen.GetComponent<Image>().color; // get the current color of blood_screen

        if (blood_screen_color.a > 0f) // if the blood_screen color's alpha is greater than 0f (min)
        {
            blood_screen_color.a -= 0.1f; // decrease opacity by 0.1f
            blood_screen.GetComponent<Image>().color = blood_screen_color; // update blood_screen color with new modified color
            Invoke(nameof(BloodScreenPulseStop), blood_cycle_speed/2); // call this function again
        }
    }

    void BloodScreen()
    {
        // if player is damaged
        if (player.GetComponent<playerStats>().playerDamaged == true)
        {
            // check if blood cycle is currently off, if it's on/true, we don't want to call this again
            if(blood_cycle_on == false)
            {
                BloodScreenPulseUp(); // start blood screen effect cycle
                blood_cycle_on = true; // set to true, this prevents this function from being called multiple times by Update
            }
            
        }
    }
}
