using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallMarker : MonoBehaviour
{

    public GameObject myNote;
    private void OnTriggerEnter(Collider other)
    {
        if(other == myNote)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if(myNote == null)
        {
            Destroy(gameObject);
        }
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
