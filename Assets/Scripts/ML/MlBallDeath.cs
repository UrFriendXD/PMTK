using System;
using System.Collections;
using System.Collections.Generic;
using ML;
using Unity.MLAgents;
using UnityEngine;

public class MlBallDeath : MonoBehaviour
{
    public bool mltrainig;
    public WitchRayAgent agent;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (mltrainig)
        {
            if (col.CompareTag("Payload"))
            {
                agent.done = true;
            }
        }
    }
}
