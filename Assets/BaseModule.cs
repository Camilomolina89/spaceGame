using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseModule : MonoBehaviour
{
    public bool selected;
    void Update()
    {
        var renderer = GetComponent<Renderer>();
        if (selected) {

            renderer.material.color = Color.red;
        } else {
            renderer.material.color = Color.white;
        }
        selected = false;
    }
}
