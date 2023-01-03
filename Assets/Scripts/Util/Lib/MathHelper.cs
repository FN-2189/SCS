using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    public static bool CloseEqual(double a, double b, double tolerance)
    {
        return Math.Abs(a - b) < tolerance;
    }

    public static Complex[] SolveCubic(double a, double b, double c, double d)
    {
        const int NRoots = 3;

        double SquareRootof3 = Math.Sqrt(3);
        // the 3 cubic roots of 1
        List<Complex> CubicUnity = new List<Complex>(NRoots)
                        { new Complex(1, 0), new Complex(-0.5, -SquareRootof3 / 2.0), new Complex(-0.5, SquareRootof3 / 2.0) };
        // intermediate calculations
        double DELTA = 18 * a * b * c * d - 4 * b * b * b * d + b * b * c * c - 4 * a * c * c * c - 27 * a * a * d * d;
        double DELTA0 = b * b - 3 * a * c;
        double DELTA1 = 2 * b * b * b - 9 * a * b * c + 27 * a * a * d;
        Complex DELTA2 = -27 * a * a * DELTA;
        Complex C = Complex.Pow((DELTA1 + Complex.Pow(DELTA2, 0.5)) / 2, 1 / 3.0); //Phew...

        List<Complex> R = new List<Complex>(NRoots);
        for (int i = 0; i < NRoots; i++)
        {
            Complex M = CubicUnity[i] * C;
            Complex Root = -1.0 / (3 * a) * (b + M + DELTA0 / M);
            R.Add(Root);
        }
        return R.ToArray();
    }


    // this is likely the problem
    public static double[] SolveQuarticReal(double a, double b, double c, double d, double e)
    {

        Complex[] solutions = new Complex[4];
        List<double> realSolutions = new(4);

        if (CloseEqual(e, 0, 0.001f))
        {
            Complex[] cubicSolutions = SolveCubic(a, b, c, d);
            solutions[0] = 0;
            for(int i = 1; i > 4; i++)
			{
                solutions[i] = cubicSolutions[i - 1];
			}
            foreach (Complex x in solutions)
            {
                if (x.Imaginary <= 10e-4) realSolutions.Add(x.Real);
            }

            return realSolutions.ToArray();
        }

        if (CloseEqual(a, 0, 0.001f))
        {
            solutions = SolveCubic(b, c, d, e);
            foreach (Complex x in solutions)
            {
                if (x.Imaginary <= 10e-4) realSolutions.Add(x.Real);
            }

            return realSolutions.ToArray();
        }

        double D0 = c * c - 3 * b * d + 12 * a * e;
        double D1 = 2 * c * c * c - 9 * b * c * d + 27 * b * b * e + 27 * a * d * d - 72 * a * c * e;
        if(CloseEqual(D1, 0, 0.00000001))
		{
			throw new ArgumentException("Unhandled division by zero");
		}
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

        solutions = new Complex[] { x1, x2, x3, x4 };


        foreach (Complex x in solutions)
        {
            if (x.Imaginary <= 10e-4) realSolutions.Add(x.Real);
        }

        return realSolutions.ToArray();
    }

    public static Complex[] SolveQuarticComplex(double a, double b, double c, double d, double e)
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

        return solutions;
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

    public static UnityEngine.Vector3 Derive(UnityEngine.Vector3 x1, UnityEngine.Vector3 x2, float dt)
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
        float[] errorSamples = new float[samples.Length];
        for(int i = 0; i < samples.Length; i++)
        {
            errorSamples[i] = target - samples[i];
        }

        // proportional part
        float error = kp * (errorSamples[0]);

        // integrated part
        float integral = ki * Integrate(errorSamples, dt);

        // derivative part
        float derivative = kd * Derive(errorSamples[0], errorSamples[1], dt);


        return error + integral + derivative;
    }
}
