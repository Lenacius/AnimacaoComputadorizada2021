using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public Image Gun;
    public Image ShotGun;
    public Image GaugeMeter;
    
    // Start is called before the first frame update
    void Start()
    {
        Gun.color = new Vector4(0, 255, 0, 255);
        ShotGun.color = new Vector4(255, 255, 255, 255);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            Gun.color = new Vector4(0, 255, 0, 255);
            ShotGun.color = new Vector4(255, 255, 255, 255);
            GaugeMeter.rectTransform.localScale = new Vector3(0.2f, 1, 1);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            Gun.color = new Vector4(255, 255, 255, 255);
            ShotGun.color = new Vector4(0, 255, 0, 255);
            GaugeMeter.rectTransform.localScale = new Vector3(0.1f, 1, 1);
        }
    }


}
