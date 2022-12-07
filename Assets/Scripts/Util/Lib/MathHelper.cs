using System;
using System.Collections.Generic;
using System.Numerics;

public static class MathHelper
{
    public static double[] SolveQuadratic(double a, double b, double c)
    {
        //Discriminant
        double D = Math.Sqrt((b * b) - 4 * a * c);

        double x1 = (-b + D) / (2 * a);
        double x2 = (-b - D) / (2 * a);

        return new double[] { x1, x2 };
    }

    public static double[] SolveQuarticReal(double a, double b, double c, double d, double e)
    {
        double D0 = c * c - 3 * b * d + 12 * a * e;
        double D1 = 2 * c * c * c - 9 * b * c * d + 27 * b * b * e + 27 * a * d * d - 72 * a * c * e;
        double p = (8 * a * c - 3 * b * b) / (8 * a * a);
        double q = (b * b * b - 4 * a * b * c + 8 * a * a * d) / (8 * a * a * a);
        Complex Q = Complex.Pow((D1 + Complex.Sqrt(D1 * D1 - 4 * D0 * D0 * D0)) / 2, 1.0 / 3.0);
        Complex S = Complex.Sqrt(-2 * p / 3 + (Q + D0 / Q) / (3 * a)) / 2;
        Complex u = Complex.Sqrt(-4 * S * S - 2 * p + q / S) / 2;
        Complex v = Complex.Sqrt(-4 * S * S - 2 * p - q / S) / 2;
        Complex x1 = -b / (4 * a) - S + u;
        Complex x2 = -b / (4 * a) - S - u;
        Complex x3 = -b / (4 * a) + S + v;
        Complex x4 = -b / (4 * a) + S - v;

        Complex[] solutions = new Complex[] { x1, x2, x3, x4 };
        List<double> realSolutions = new(4);

        foreach(Complex x in solutions)
        {
            if (x.Imaginary <= 10e-4) realSolutions.Add(x.Real);
        }

        return realSolutions.ToArray();
    }

    public static double GetLowestPositive(double[] values)
    {
        double lowest = double.PositiveInfinity;
        foreach (double x in values)
        {
            if (x >= 0f) lowest = Math.Min(lowest, x);
        }
        return lowest == double.PositiveInfinity ? -1 : lowest;
    }

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
