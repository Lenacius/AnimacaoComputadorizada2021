using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeHandler : MonoBehaviour
{
    SlimeController SlimeScript;
    // Start is called before the first frame update
    void Start()
    {
        SlimeScript = GetComponentInParent<SlimeController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Move()
    {
        SlimeScript.Move();
    }
    void Stop()
    {
        SlimeScript.Stop();
    }

}
