using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public TMP_Text fpsCounterDisplay;

    public int sampleSize = 10;

    private float[] lastDeltaTimes;


    void Awake()
    {
        lastDeltaTimes = new float[sampleSize];
    }

    // Update is called once per frame
    void Update()
    {
        float currentDeltaTime = Time.unscaledDeltaTime;
        for (int i = lastDeltaTimes.Length - 1; i >= 1; i--)
        {
            lastDeltaTimes[i] = lastDeltaTimes[i - 1];
        }
        lastDeltaTimes[0] = currentDeltaTime;

        //get average
        float sum = 0f;
        foreach (float delta in lastDeltaTimes) sum += delta;
        float average = sum / lastDeltaTimes.Length;

        float fps = 1 / average;

        fpsCounterDisplay.text = "FPS: " + ((int)fps).ToString();
    }
}
