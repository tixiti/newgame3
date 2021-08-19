using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadEffect : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            // gameObject.SetActive(false);
            var main = GetComponent<ParticleSystem>().main;
            main.simulationSpeed = 5;
        }
    }

    private void OnEnable()
    {
        var main = GetComponent<ParticleSystem>().main;
        main.simulationSpeed = 1;
    }
}
