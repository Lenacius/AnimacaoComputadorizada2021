using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
// Movement
    public float MoveSpeed = 5;
    public float RotSpeed = 100;

// Particles
    ParticleSystem playerParticles;
    bool side; // true = front | false = back
    bool still;
    public ParticleSystem gunParticles;
    // Shot
    GameObject HoldingGun;
    GameObject Gun;
    GameObject ShotGun;
    public GameObject Ammo;
    public bool ShotEventTrigger;
    public bool Shotted;
    void OnEnable()
    {
        GameController.Beat += Shot;
    }
    void OnDisable()
    {
        GameController.Beat -= Shot;
    }

    // Start is called before the first frame update
    void Start()
    {
        HoldingGun = this.gameObject.transform.GetChild(1).gameObject;
        Gun = GameObject.FindGameObjectWithTag("Weapon1");
        ShotGun = GameObject.FindGameObjectWithTag("Weapon2");

        playerParticles = GetComponent<ParticleSystem>();
        gunParticles = Gun.GetComponent<ParticleSystem>();

        ShotGun.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        ParticlePosition();
        SelectGun();
    }
    
    void SelectGun()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            Gun.SetActive(true);
            ShotGun.SetActive(false);
            gunParticles = Gun.GetComponent<ParticleSystem>();
            HoldingGun = Gun;
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            Gun.SetActive(false);
            ShotGun.SetActive(true);
            gunParticles = ShotGun.GetComponent<ParticleSystem>();
            HoldingGun = ShotGun;
        }

    }

    void Move()
    {
        Vector3 movement = new Vector3();

        if (Input.GetKey(KeyCode.W))
        {
            movement += transform.forward * Time.deltaTime * MoveSpeed;
            side = false;
            still = false;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movement += -transform.forward * Time.deltaTime * MoveSpeed;
            side = true;
            still = false;
        }
        else
            still = true;

        transform.position += movement;

        if (Input.GetKey(KeyCode.D))
            transform.Rotate(new Vector3(0, Time.deltaTime * RotSpeed, 0));
        else if (Input.GetKey(KeyCode.A))
            transform.Rotate(new Vector3(0, -Time.deltaTime * RotSpeed, 0));

    }

    void ParticlePosition()
    {
        var shape = playerParticles.shape;
        if (side)
        {
            shape.position = new Vector3(0, -0.4f, 0.5f);
            shape.scale = new Vector3(1, .2f, 1);
        }
        else
        {
            shape.position = new Vector3(0, -0.4f, -0.5f);
            shape.scale = new Vector3(1, .2f, -1);
        }

        if (still)
            playerParticles.Stop(false);
        else
            playerParticles.Play(false);
    }

    void Shot()
    {
        if (HoldingGun == ShotGun)
        {
            GameObject bullet = Instantiate(Ammo, HoldingGun.transform.GetChild(0).gameObject.transform.position, HoldingGun.transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 1000);
            ShotFlare();
        }
        if (!ShotEventTrigger && !Shotted && HoldingGun == Gun)
        {
            GameObject bullet = Instantiate(Ammo, HoldingGun.transform.GetChild(0).gameObject.transform.position, HoldingGun.transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 1000);
            ShotFlare();
            Shotted = true;
        }
    }

    

    void ShotFlare() {
        gunParticles.Play(false);
    }
}
