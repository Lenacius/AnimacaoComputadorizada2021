using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //IARelevant
    public int commandMovement = 0;
    public int commandRotate = 0;
    public int commandWork = 0;
    //ControllerRelavant
    public const float MAX_ENERGY = 100.0f;
    public float energy = MAX_ENERGY;
    public const float MAX_HUNGRY = 100.0f;
    public float hungry = 0;
    public const float MAX_MOOD = 100.0f;
    public float mood = MAX_MOOD;

    public float money = 0;

    float rotSpeed = 100;
    const int MAX_SPEED_MULTIPLIER = 2;
    float movMultiplier = 2;
    const int MIN_SPEED = 2;

    public bool inWork = false;
    public bool inPlay = false;
    public bool inSleep = false;
    public bool inLunch = false;

    public bool working = false;
    public bool playing = false;
    public bool eating = false;
    public bool sleeping = false;

    public bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("EnergyCountdown");
        StartCoroutine("HungryCountdown");
        StartCoroutine("MoodCountdown");
    }

    // Update is called once per frame
    void Update()
    {
        if(!dead)
        {
            PlayerInput();

            movMultiplier = MAX_SPEED_MULTIPLIER * (energy / 100.0f);

            if (!inWork) StopCoroutine("Working");
            if (!inPlay) StopCoroutine("Playing");
            if (!inLunch) StopCoroutine("Eating");
            if (!inSleep) StopCoroutine("Sleeping");

            CheckMinMax();
        }
    }

    void PlayerInput()
    {
        if (Input.GetKey(KeyCode.W) || commandMovement == 1)
        {
            this.transform.Translate(this.transform.forward * Time.deltaTime * (movMultiplier + MIN_SPEED), Space.World);
            energy -= Time.deltaTime / 2;
        }
        if (Input.GetKey(KeyCode.A) || commandRotate == 1)
        {
            this.transform.Rotate(new Vector3(0, -rotSpeed * Time.deltaTime, 0), Space.World);
        }
        if (Input.GetKey(KeyCode.D) || commandRotate == 2)
        {
            this.transform.Rotate(new Vector3(0, rotSpeed * Time.deltaTime, 0), Space.World);
        }

        if (Input.GetKeyDown(KeyCode.Space) || commandWork == 1)
        {
            if (inWork && !working)
            {
                working = true;
                StartCoroutine("Working");
            }

            if (inPlay && money >= 2.0f && !playing)
            {
                playing = true;
                StartCoroutine("Playing");
            }

            if (inLunch && money >= 0.5f && !eating)
            {
                eating = true;
                StartCoroutine("Eating");
            }

            if (inSleep && !sleeping)
            {
                sleeping = true;
                StartCoroutine("Sleeping");
            }
        }

    }

    void CheckMinMax()
    {
        if (energy > MAX_ENERGY) energy = MAX_ENERGY;
        if (mood > MAX_MOOD) mood = MAX_MOOD;
        if (hungry < 0) hungry = 0;
        if (money < 0) money = 0;

        if (energy < 0 || mood < 0 || hungry > 100) dead = true;
    }

    private IEnumerator Working()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            money += 5;
            mood -= 5f;
            hungry += 1;
            energy -= 3;
        }
    }
    private IEnumerator Playing()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            mood += 5;
            energy -= 2;
            hungry += 2;
            money -= 2;
        }
    }
    private IEnumerator Eating()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            hungry -= 5;
            mood += 1;
            energy += 1;
            money -= 0.5f;
        }
    }
    private IEnumerator Sleeping()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            energy += 5;
            mood += 2;
            hungry += 1;
        }
    }

    private IEnumerator EnergyCountdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(5.0f);
            energy -= 1;
        }
    }

    private IEnumerator HungryCountdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(10.0f);
            hungry += 1;
        }
    }

    private IEnumerator MoodCountdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(20.0f);
            mood -= 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Work")
            inWork = true;
        if (other.tag == "Play")
            inPlay = true;
        if (other.tag == "Eat")
            inLunch = true;
        if (other.tag == "Sleep")
            inSleep = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Work")
        {
            inWork = false;
            working = false;
        }
        if (other.tag == "Play")
        {
            inPlay = false;
            playing = false;
        }
        if (other.tag == "Eat")
        {
            inLunch = false;
            eating = false;
        }
        if (other.tag == "Sleep")
        {
            inSleep = false;
            sleeping = false;
        }
    }

    public void StopAnyCoroutine()
    {
        StopCoroutine("Working");
        StopCoroutine("Playing");
        StopCoroutine("Eating");
        StopCoroutine("Sleeping");
        transform.localPosition = Vector3.zero;
    }
}
