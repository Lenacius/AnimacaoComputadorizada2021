using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Player;

    public GameObject Enemy;
    float spawn_rate = 5;
    float spawn_time = 5;

    public bool gauge_up;
    public int spawn_gauge = 0;

    public float spawned_speed = 1;
    void OnEnable()
    {
        GameController.Beat += SpawnByGauge;
    }
    void OnDisable()
    {
        GameController.Beat -= SpawnByGauge;
    }
    // Start is called before the first frame update
    void Start()
    {
        spawn_time = spawn_rate;
    }

    // Update is called once per frame
    void Update()
    {
        //SpawnByTime();
        //SpawnByGauge();
    }

    void SpawnByGauge()
    {
        if (!gauge_up)
        {
            spawn_gauge += 1;
            if (spawn_gauge >= 8)
            {
                InstantiateEnemy(spawned_speed);
                spawn_gauge = 0;
            }
        }
    }

    void SpawnByTime()
    {
        spawn_time -= Time.deltaTime;
        if (spawn_time <= 0)
        {
            InstantiateEnemy(1);
            spawn_time = spawn_rate;
        }
    }

    void InstantiateEnemy(float Speed)
    {
        float posX = Random.Range(-50, 50);
        float posY = Random.Range(-50, 50);

        GameObject newEnemy = Instantiate(Enemy, new Vector3(posX, 1, posY), transform.rotation);
        newEnemy.GetComponent<SlimeController>().Speed = Speed * 1.5f;
        newEnemy.GetComponentInChildren<Animator>().enabled = true;
        newEnemy.GetComponentInChildren<Animator>().speed = Speed;
        newEnemy.GetComponent<SlimeController>().Player = Player;
    }
}
