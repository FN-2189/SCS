using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper
{
    public static float Derive(float x1, float x2, float dt)
    {
        return (x1 - x2) / dt;
    }

    public static float Integrate(float[] samples, float dt)
    {
        float result = 0f;

        foreach(float x in samples)
        {
            result += x * dt;
        }

        return result;
    }

    public static float PID(float kp, float ki, float kd, float[] samples, float target, float dt)
    {
        // output: kp * (target - current) + ki * integrate samples + kd * derive last 2 values

        // proportional part
        float error = kp * (target - samples[0]);

        // integrated part
        float i = ki * Integrate(samples, dt);

        // derivative part
        float d = kd * Derive(samples[0], samples[1], dt);


        return error + i + d;
    }
}
