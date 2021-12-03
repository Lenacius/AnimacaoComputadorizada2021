using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public float energy;
    public float mood;
    public float hungry;
    public float money;

    public Scrollbar energySB;
    public Scrollbar moodSB;
    public Scrollbar hungrySB;
    public Image energyBG;
    public Image moodBG;
    public Image hungryBG;
    public Text moneyPH;

    public GameObject deathPanel;
    public GameObject winningPanel;

    // Start is called before the first frame update
    void Start()
    {
        deathPanel.SetActive(false);
        winningPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        energySB.value = energy / 100.0f;
        moodSB.value = mood / 100.0f;
        hungrySB.value = hungry / 100.0f;
        moneyPH.text = money.ToString();

        ScrollbarColor(energySB, energyBG);
        ScrollbarColor(moodSB, moodBG);
        ScrollbarHungry();
    }

    void ScrollbarColor(Scrollbar scroll, Image image)
    {
        float aux = scroll.value * 100.0f;
        float red = 0;
        float green = 0;

        if(aux < 50)
        {
            red = 1.0f;
            green = (aux / 50.0f);
        }
        else
        {
            green = 1.0f;
            red = 1.0f - ((aux - 50.0f) / 50.0f);
        }

        image.color = new Color(red, green, 0, 1);
    }

    void ScrollbarHungry()
    {
        float red = 0;
        float green = 0;

        if (hungry < 50)
        {
            green = 1.0f;
            red = (hungry / 50.0f);
        }
        else
        {
            red = 1.0f;
            green = 1.0f - ((hungry - 50.0f) / 50.0f);
        }

        hungryBG.color = new Color(red, green, 0, 1);
    }

    public void WinnerScreen(bool screen_state)
    {
        winningPanel.SetActive(screen_state);
    }

    public void LoserScreen(bool screen_state)
    {
        deathPanel.SetActive(screen_state);
    }
}
