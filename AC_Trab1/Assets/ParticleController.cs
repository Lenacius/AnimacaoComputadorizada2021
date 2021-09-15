using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    AnimationController animCon;
    public ParticleSystem parSysM;
    public ParticleSystem parSysJ;
    public ParticleSystem parSysC;
    public bool stop;
    public float stopTime = 0.5f;
    float clock = 0;

    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        animCon = GetComponent<AnimationController>();
        var emission = parSysJ.emission;
        emission.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        StopTime();
    }

    void Movement()
    {
        var emission = parSysM.emission;
        var shape = parSysM.shape;
        ParticleSystem.ShapeModule eshape = shape;

        if (animCon.moving)
            emission.enabled = true;
        else
            emission.enabled = false;

        if (animCon.d)
        {
            eshape.position = new Vector3(0, 0.5f, -0.5f);
            eshape.scale = new Vector3(1, 1, -1);
        }
        else
        {
            eshape.position = new Vector3(0, 0.5f, 0.5f);
            eshape.scale = new Vector3(1, 1, 1);
        }
    }

    void StopTime()
    {
        var emission = parSysJ.emission;

        if (!stop)
        {
            clock += Time.deltaTime;
            if(clock >= stopTime)
            {
                emission.enabled = false;
                stop = true;
                clock = 0;
                parSysJ.Clear();
            }
        }
    }

    public void Land()
    {
        var emission = parSysJ.emission;

        parSysJ.Play();
        emission.enabled = true;
        stop = false;
    }

    public void Clicked()
    {
        var emission = parSysC.emission;

        parSysC.Play();
        emission.enabled = true;
    }
    public void NClicked()
    {
        var emission = parSysC.emission;

        parSysC.Clear();
        emission.enabled = false;
    }

}
