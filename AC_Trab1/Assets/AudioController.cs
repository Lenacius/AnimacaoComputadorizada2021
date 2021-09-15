using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource audSrc;
    public AudioSource audSrc2;
    public AudioSource audSrc3;
    public AudioSource audSrc4;
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void JumpSound()
    {
        audSrc.Play(0);
    }

    public void Run()
    { 
        audSrc2.Play(0);
        audSrc2.loop = true;
    }
    public void Stop()
    {
        audSrc2.Stop();
    }

    public bool Check2()
    {
        return audSrc2.isPlaying;
    }

    public void Correct()
    {
        audSrc3.Play(0);
    }

    public void Click()
    {
        audSrc4.Play(0);
        Debug.Log("aa");
    }
}
