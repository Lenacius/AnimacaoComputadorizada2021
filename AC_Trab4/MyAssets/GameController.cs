using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public UIController ui;
    public Animator anim;
    public PlayerController player;

    public Camera topCamera;
    public Camera charCamera;

    private int state;
    // Start is called before the first frame update
    void Start()
    {
        charCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();

        UpdateAnimator();

        ChangeCamera();

        if (player.dead)
        {
            ui.LoserScreen(true);
            StartCoroutine("RestartScene");
        }
        if(player.money > 250)
        {
            ui.WinnerScreen(true);
            StartCoroutine("RestartScene");
        }
    }

    void UpdateUI()
    {
        ui.mood = player.mood;
        ui.energy = player.energy;
        ui.hungry = player.hungry;
        ui.money = player.money;
    }

    void UpdateAnimator()
    {
        if (player.energy > 50 && player.mood > 50)
            anim.SetInteger("State", 0);
        else if (player.energy > 50 && player.mood < 50)
            anim.SetInteger("State", 1);
        else if (player.energy < 50 && player.mood > 50 && player.hungry < 50)
            anim.SetInteger("State", 2);
        else if (player.energy < 50 && player.mood > 50 && player.hungry > 50)
            anim.SetInteger("State", 4);
    }

    void ChangeCamera()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            topCamera.enabled = !topCamera.enabled;
            charCamera.enabled = !charCamera.enabled;
        }
    }

    private IEnumerator RestartScene()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            resetGame();
        }
    }

    private void resetGame()
    {
        player.energy = PlayerController.MAX_ENERGY;
        player.mood = PlayerController.MAX_MOOD;
        player.hungry = 0;
        player.money = 0;
        player.dead = false;
        ui.LoserScreen(false);
        ui.WinnerScreen(false);
        StopCoroutine("RestartScene");
    }
}