using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUITest : MonoBehaviour
{
    // objects
    public GameObject player; // player
    public GameObject stamina_bar; // stamina ui element
    public GameObject blood_screen; // blood screen ui element
    public GameObject interact_e; // interact E ui element
    public GameObject hint; // hint from clue that was picked up


    // script local variables
    private float normalized_stamina; // stamina of player, but normalized to 0-1
    private bool blood_cycle_on; // true if the blood screen currently on/cycle is running
    private float blood_cycle_speed; // speed of blood cycle/pulse
    private bool show_interact_ui; // true if the interact UI [E] is being shown
    private bool show_hint; // true if clue has been picked up and within clue collider

    // Start is called before the first frame update
    void Start()
    {
        // find objects
        player = GameObject.Find("Player");
        stamina_bar = GameObject.Find("stamina");
        blood_screen = GameObject.Find("blood_screen");
        interact_e = GameObject.Find("interact_e");
        // blood screen initialization
        blood_cycle_on = false;
        blood_cycle_speed = 0.08f;
        // interact ui initialization
        show_interact_ui = false;
    }

    // Update is called once per frame
    void Update()
    {
        // stamina bar
        if(player.GetComponent<PlayerStatsTest>().playerStamina < 100){ // if player is using stamina
            stamina_bar.SetActive(true); // show stamina UI
            StaminaBar(); // update stamina bar
        }
        else{
            stamina_bar.SetActive(false); // show stamina UI
        }

        // blood screen
        if (player.GetComponent<PlayerStatsTest>().playerDamaged == true){ // if player is damaged
            BloodScreen(); // show blood screen
        }

        // display interact UI
        if (show_interact_ui == true){
            interact_e.SetActive(true); // show interact UI
        }
        else{
            interact_e.SetActive(false); // hide interact UI
        }
    }

    void StaminaBar()
    {
        // normalize player stamina from 0-100 to 0-1
        normalized_stamina = (player.GetComponent<PlayerStatsTest>().playerStamina) / (player.GetComponent<PlayerStatsTest>().maxStamina);
        // set bar to player stamina
        //stamina_bar.GetComponent<Image>().fillAmount = normalized_stamina;
        stamina_bar.transform.GetChild(1).GetComponent<Image>().fillAmount = normalized_stamina;

        if(player.GetComponent<PlayerStatsTest>().isExausted == false) // if player has not used entire stamina bar
        {
            stamina_bar.transform.GetChild(1).GetComponent<Image>().color = new Color32(255, 255, 255, 255); // show white bar
        }
        else // if player has used entire stamina bar
        {
            stamina_bar.transform.GetChild(1).GetComponent<Image>().color = new Color32(188, 188, 188, 255); // show red bar
        }
    }

    // changes blood screen so it is high opacity
    void BloodScreenPulseUp()
    {
        //Debug.Log("PulseUp");
        var blood_screen_color = blood_screen.GetComponent<Image>().color; // get the current color of blood_screen

        if (player.GetComponent<PlayerStatsTest>().playerDamaged == true) // if player is damaged
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
        //Debug.Log("PulseDown");
        var blood_screen_color = blood_screen.GetComponent<Image>().color; // get the current color of blood_screen

        if (player.GetComponent<PlayerStatsTest>().playerDamaged == true) // if player is damaged
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
        //Debug.Log("PulseStop");
        var blood_screen_color = blood_screen.GetComponent<Image>().color; // get the current color of blood_screen

        if (blood_screen_color.a > 0f) // if the blood_screen color's alpha is greater than 0f (min)
        {
            blood_screen_color.a -= 0.1f; // decrease opacity by 0.1f
            blood_screen.GetComponent<Image>().color = blood_screen_color; // update blood_screen color with new modified color
            Invoke(nameof(BloodScreenPulseStop), blood_cycle_speed/2); // call this function again
            blood_cycle_on = false;
        }
    }

    void BloodScreen()
    {
        // check if blood cycle is currently off, if it's on/true, we don't want to call this again
        if(blood_cycle_on == false)
        {
            BloodScreenPulseUp(); // start blood screen effect cycle
            blood_cycle_on = true; // set to true, this prevents this function from being called multiple times by Update
        }
    }

    // public API for displaying interact UI: [E] 'text'
    public void DisplayInteractUI(string interactText)
    {
        show_interact_ui = true; // enable show interact bool, this will trigger Update function to enable interact ui game object
        interact_e.GetComponent<TextMeshProUGUI>().text = "[E] " + interactText; // change text to "[E] interactText" (ie: [E] Hide)
    }

    // public API for disabling interact UI
    public void DisableInteractUI()
    {
        show_interact_ui = false; // disable show interact bool, this will trigger Update function to enable interact ui game object
        interact_e.GetComponent<TextMeshProUGUI>().text = "[E]"; // reset text to just "[E]"
    }

    public void DisableHintUI()
    {
        show_hint = false;
    }
}
