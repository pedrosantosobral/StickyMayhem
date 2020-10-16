using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public void SetMenuCameraPos()
    {
        gameObject.transform.position = new Vector3(-10, 6.5f, -5f);
    }

    public void SetPlayingCameraPos()
    {
        gameObject.transform.position = new Vector3(2, 6.5f, 3.5f);
    }

    private void Start()
    {
        SetMenuCameraPos();
    }
}
