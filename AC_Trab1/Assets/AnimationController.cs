using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator controller;
    public Camera cam;
    public GameObject target;
    public bool jump;
    public bool click;
    public bool d;
    public bool moving;
    public ParticleController parCon;
    public AudioController audCon;
    public bool once = false;

    // Start is called before the first frame update
    void Start()
    {
        controller.SetBool("Jump", false);
        controller.SetBool("Start", false);
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        if (!moving)
        {
            Correction();
        }

        controller.SetBool("Jump", jump);
        controller.SetBool("Start", click);

        cam.transform.position = new Vector3(-11, 3.5f, -2) + target.transform.position;
    }

    void MyInput()
    {
        jump = Input.GetKey(KeyCode.Space);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //Select stage    
                if (hit.transform.name == "Cube")
                {
                    click = true;
                    audCon.Click();
                }
                else
                    click = false;
            }
        }
        else click = false;

        if (Input.GetKey(KeyCode.A))
        {
            moving = true;
            Move(-1);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moving = true;
            Move(1);
        }
        else
        {
            moving = false;
        }

        if (moving)
        {
            if (!audCon.Check2())
                playRun();
        }
        else
        {
            playStop();
        }
    }

    void Move(int dir)
    {

        Mesh mesh = target.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        Vector3 aux = normals[1] * Mathf.Sin(Time.deltaTime) * dir;

        if (Vector3.Distance(vertices[0], vertices[2]) < 1.5f)
            for (var i = 2; i < 4; i++)
            {
                vertices[i] += aux;
                vertices[i + 2] += aux;
                vertices[i + 6] += aux;
                vertices[i + 8] += aux;
                vertices[i + 15] += aux;
                vertices[i + 19] += aux;
            }

        transform.position += aux * -10;

        mesh.vertices = vertices;

        if (dir <= 0)
            d = true;
        else
            d = false;
    }

    void Correction()
    {
        int dir = 1;
        if (d) dir = 1;
        else dir = -1;

        Mesh mesh = target.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        if (Vector3.Distance(vertices[0], vertices[2]) > 1.0002f)
        {
            if (!audCon.audSrc3.isPlaying && !once)
            {
                once = true;
                audCon.Correct();
            }

            for (var i = 2; i < 4; i++)
            {
                float aux = Mathf.Sin(Time.deltaTime) * dir * 10;
                vertices[i] += normals[i] * aux;
                vertices[i + 2] += normals[i] * aux;
                vertices[i + 6] += normals[i] * aux;
                vertices[i + 8] += normals[i] * aux;
                vertices[i + 15] += normals[i] * aux;
                vertices[i + 19] += normals[i] * aux;
            }
        }
        else
            once = false;

        mesh.vertices = vertices;
    }

    void Land()
    {
        parCon.Land();
    }

    void playJump()
    {
        audCon.JumpSound();
    }

    void playRun()
    {
        audCon.Run();
    }
    void playStop()
    {
        audCon.Stop();
    }

    void playClick()
    {
        parCon.Clicked();
    }
    void stopClick()
    {
        parCon.NClicked();
    }
}
