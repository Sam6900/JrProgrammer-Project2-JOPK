using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimeBar : MonoBehaviour
{
    [SerializeField] int totalLevelTime;
    private Slider slider;

    public static event Action TimeBarFinished;

    void Start()
    {
        slider = GetComponent<Slider>();

        // Sets up slider here
        slider.maxValue = totalLevelTime;
        slider.value = totalLevelTime;
        
        StartCoroutine(Timer());
    }

    void Update()
    {
        
    }

    // Starts counting down from levelTime to 0
    public IEnumerator Timer()
    {
        while (slider.value >= 0)
        {
            yield return new WaitForSeconds(0.5f);

            if (PlayerController.isPlayerAlive == true)
                slider.value -= 0.5f;
            
            if(slider.value <= 0)
            {
                if (TimeBarFinished != null)
                    TimeBarFinished();
            }
        }
    }
}
