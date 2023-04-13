using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class World : MonoBehaviour
{
    [SerializeField]
    private float counter = 0f;

    [SerializeField]
    float tickTime = 1f;
    public delegate void TickAction();
    public static event TickAction onTick;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter >= tickTime)
        {
            counter = 0;
            if (onTick != null) {
                onTick();
                Debug.Log("TICK");
            }
        }
    }
}

