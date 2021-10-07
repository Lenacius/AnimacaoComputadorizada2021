using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    //Status
    public int slimeLife = 20;

    //Movement
    public float Speed = 1;
    public bool moving = false;

    //Animation
    Animator SlimeAnimator;
    public float animationSpeed = 1;

    //Particles
    ParticleSystem SlimeParticles;

    //Chase
    public GameObject Player;

    void Start()
    {
        SlimeAnimator = GetComponentInChildren<Animator>();
        SlimeParticles = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        SlimeAnimator.SetFloat("Period", animationSpeed);

        if (moving)
            transform.position += transform.forward * Speed * Time.deltaTime;
    }

    public void Move()
    {
        moving = true;
    }
    public void Stop()
    {
        moving = false;
        transform.LookAt(Player.transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {

            SlimeParticles.Play();
            var parColor = SlimeParticles.main;
            parColor.startColor = GetComponentInChildren<MeshRenderer>().material.color;
            //Debug.Log(parColor);
            slimeLife--;
            Destroy(collision.gameObject);

            if (slimeLife >= 15 && slimeLife < 20)
            {
                GetComponentInChildren<MeshRenderer>().material.color = new Color(179f / 255f, 255f / 255f, 4f / 255f, 150f / 255f);
            }
            else if (slimeLife >= 10 && slimeLife < 15)
            {
                GetComponentInChildren<MeshRenderer>().material.color = new Color(255f / 255f, 255f / 255f, 4f / 255f, 150f / 255f);
            }
            else if (slimeLife >= 5 && slimeLife < 10)
            {
                GetComponentInChildren<MeshRenderer>().material.color = new Color(255f / 255f, 167f / 255f, 4f / 255f, 150f / 255f);
            }
            else if (slimeLife >= 1 && slimeLife < 5)
            {
                GetComponentInChildren<MeshRenderer>().material.color = new Color(255f / 255f, 83f / 255f, 4f / 255f, 150f / 255f);
            }
            else if (slimeLife == 0)
            {
                Destroy(gameObject);
            }


        }


    }

}
