using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public delegate void BeatDetection();
    public static event BeatDetection Beat;

    MusicController musicController;

    GameObject player;
    PlayerController playerController;

    GameObject mainCamera;

    public GameObject musicGaugeA;
    public GameObject musicGaugeB;

    Spawner spawner;
    public GameObject spawnGauge;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        player = GameObject.FindGameObjectWithTag("Player");        
        playerController = player.GetComponent<PlayerController>();

        musicController = GetComponent<MusicController>();

        spawner = GetComponentInChildren<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (musicController.trigger)
        {
            if (Beat != null)
                Beat();
            StartCoroutine(this.ScreenShake(.01f, .1f));
        }

        FollowPlayer(mainCamera.transform);

        playerController.ShotEventTrigger = musicController.trigger;
        playerController.Shotted = musicController.trigger;
        spawner.gauge_up = musicController.trigger;

        musicGaugeA.transform.localScale = new Vector3(musicController.audioBandBuffer[0], 1, 1);
        musicGaugeB.transform.localScale = new Vector3(musicController.audioBandBuffer[1], 1, 1);

        spawnGauge.transform.localScale = new Vector3(spawner.spawn_gauge / 4.0f, 1, 1);
        spawner.spawned_speed = musicController.period;
    }

    void FollowPlayer(Transform follower)
    {
        GameObject aux = GameObject.FindGameObjectWithTag("CameraPlaceHolder");
        follower.transform.position = aux.transform.position;
        follower.transform.rotation = aux.transform.rotation;
    }

    IEnumerator ScreenShake(float duration, float magnetude)
    {
        Vector3 originalPos = mainCamera.transform.position;

        float elapsed = 0.0f;
        
        while(elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnetude;
            float y = Random.Range(-1f, 1f) * magnetude;

            mainCamera.transform.position += new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }

        mainCamera.transform.position = originalPos;
    }

}
