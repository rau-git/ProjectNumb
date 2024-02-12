using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class GrowSpawn : NetworkBehaviour
{
    [SerializeField] private float step;
    private Vector3 initialScale = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector3 finalScale = new Vector3(1.0f, 1.0f, 1.0f);
    
    [Server]
    private void Start()
    {
        StartCoroutine(GrowObject());
    }

    IEnumerator GrowObject()
    {
        float current = 0f;
        float final = 1f;

        while (current < final)
        {
            transform.localScale = Vector3.Lerp(initialScale, finalScale, current);
            current += step;
            yield return null;
        }

        // redundant but just to make sure
        transform.localScale = finalScale;

        Destroy(this);
    }
}
