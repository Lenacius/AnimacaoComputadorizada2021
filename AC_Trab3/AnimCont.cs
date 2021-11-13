using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCont : MonoBehaviour
{
    public Animator animControl;

    public bool anime1 = false;
    public bool anime2 = false;
    public bool anime3 = false;
    public bool anime4 = false;
    public bool anime5 = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            anime1 = !anime1;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            anime2 = !anime2;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            anime3 = !anime3;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            anime4 = !anime4;
        if (Input.GetKeyDown(KeyCode.Alpha5))
            anime5 = !anime5;

        animControl.SetBool("Anim1", anime1);
        animControl.SetBool("Anim2", anime2);
        animControl.SetBool("Anim3", anime3);
        animControl.SetBool("Anim4", anime4);
        animControl.SetBool("Anim5", anime5);
    }
}
