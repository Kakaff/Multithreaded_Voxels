using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public enum InterpolationMethod
    {
        Linear,
        Spline,
    }

    public abstract class PointField
    {
        public abstract float GetValue(float x, float z, InterpolationMethod method);
        public abstract float GetValue(float x, float z, float length, InterpolationMethod method);

        public static float LinearInterpolation(float f1, float f2, float t)
        {
            return (f1 * (1f - t)) + (f2 * t);
        }

        public static float SmoothInterpolation(float f1, float f2, float t)
        {
            return Mathf.SmoothStep(f1, f2, t);
        }

        public static float SmootherSplineInterpolation(float f1, float f2, float f3, float f4, float f5, float t)
        {
            float s1 = LinearInterpolation(f1, f2, t);
            float s2 = LinearInterpolation(f2, f3, t);
            float s3 = LinearInterpolation(f3, f4, t);
            float s4 = LinearInterpolation(f4, f5, t);

            float s5 = LinearInterpolation(s1, s2, t);
            float s6 = LinearInterpolation(s2, s3, t);
            float s7 = LinearInterpolation(s3, s4, t);

            float s8 = SmoothInterpolation(s5, s6, t);
            float s9 = SmoothInterpolation(s6, s7, t);

            return SmoothInterpolation(s8, s9, t);
        }

        public static float LinearSplineInterpolation(float f1, float f2, float f3, float f4, float f5, float t)
        {
            float s1 = LinearInterpolation(f1, f2, t);
            float s2 = LinearInterpolation(f2, f3, t);
            float s3 = LinearInterpolation(f3, f4, t);
            float s4 = LinearInterpolation(f4, f5, t);

            float s5 = LinearInterpolation(s1, s2, t);
            float s6 = LinearInterpolation(s2, s3, t);
            float s7 = LinearInterpolation(s3, s4, t);

            float s8 = LinearInterpolation(s5, s6, t);
            float s9 = LinearInterpolation(s6, s7, t);

            return LinearInterpolation(s8, s9, t);
        }

        public static float SmootherSplineInterpolation(float f1, float f2, float f3,float t)
        {
            float s1 = SmoothInterpolation(f1, f2, t);
            float s2 = SmoothInterpolation(f2, f3, t);

            return LinearInterpolation(s1, s2, t);
        }

        public static float LinearSplineInterpolation(float f1, float f2, float f3, float f4, float t)
        {
            float s1 = LinearInterpolation(f1, f2, t);
            float s2 = LinearInterpolation(f2, f3, t);
            float s3 = LinearInterpolation(f3, f4, t);

            float s4 = LinearInterpolation(s1, s2, t);
            float s5 = LinearInterpolation(s2, s3, t);

            return LinearInterpolation(s4, s5, t);
        }

        public static float LinearSplineInterpolation(float f1, float f2, float f3, float t)
        {
            float s1 = LinearInterpolation(f1, f2, t);
            float s2 = LinearInterpolation(f2, f3, t);

            return LinearInterpolation(s1, s2, t);
        }
    }

    public sealed class PointField2x2 : PointField
    {
        float[,] values;
        public PointField2x2(float x0z0, float x1z0,
                             float x0z1, float x1z1)
        {
            values = new float[,] { { x0z0,x0z1},
                                    { x1z0,x1z1}};
        }

        public override float GetValue(float x, float z, float length, InterpolationMethod method)
        {
            float xT = x / length;
            float x0 = LinearInterpolation(values[0,0], values[1,0], xT);
            float x1 = LinearInterpolation(values[0,1], values[1,1], xT);

            return LinearInterpolation(x0, x1, z / length);
        }

        public override float GetValue(float x, float z, InterpolationMethod method)
        {
            return GetValue(x, z, 2f, method);
        }
    }
    public sealed class PointField3x3 : PointField
    {
        float[,] values;

        public PointField3x3(float x0z0, float x1z0, float x2z0,
                             float x0z1, float x1z1, float x2z1,
                             float x0z2, float x1z2, float x2z2)
        {
            values = new float[,] { {x0z0,x0z1,x0z2},
                                    {x1z0,x1z1,x1z2},
                                    {x2z0,x2z1,x2z2}};
        }

        public override float GetValue(float x, float z, InterpolationMethod method)
        {
            return GetValue(x, z, 3f, method);
        }

        public override float GetValue(float x, float z, float length, InterpolationMethod method)
        {
            float xT = (float)x / length;
            float x0 = SmootherSplineInterpolation(values[0, 0], values[1, 0], values[2, 0], xT);
            float x1 = SmootherSplineInterpolation(values[0, 1], values[1, 1], values[2, 1], xT);
            float x2 = SmootherSplineInterpolation(values[0, 2], values[1, 2], values[2, 2], xT);

            return SmootherSplineInterpolation(x0, x1, x2, (float)z / length);
        }
    }

    
    public sealed class PointField4x4 : PointField
    {
        float[,] values;

        public PointField4x4(float x0z0, float x1z0, float x2z0, float x3z0,
                             float x0z1, float x1z1, float x2z1, float x3z1,
                             float x0z2, float x1z2, float x2z2, float x3z2,
                             float x0z3, float x1z3, float x2z3, float x3z3)
        {
            values = new float[,] { {x0z0,x0z1,x0z2,x0z3},
                                    {x1z0,x1z1,x1z2,x1z3},
                                    {x2z0,x2z1,x2z2,x2z3},
                                    {x3z0,x3z1,x3z2,x3z3}};
        }

        public override float GetValue(float x, float z, InterpolationMethod method)
        {
            return GetValue(x, z, 4f, method);
        }

        public override float GetValue(float x, float z, float length, InterpolationMethod method)
        {
            float xT = (float)x / length;

            float x0 = LinearSplineInterpolation(values[0, 0], values[1, 0], values[2, 0], values[3, 0], xT);
            float x1 = LinearSplineInterpolation(values[0, 1], values[1, 1], values[2, 1], values[3, 1], xT);
            float x2 = LinearSplineInterpolation(values[0, 2], values[1, 2], values[2, 2], values[3, 2], xT);
            float x3 = LinearSplineInterpolation(values[0, 3], values[1, 3], values[2, 3], values[3, 3], xT);

            return LinearSplineInterpolation(x0, x1, x2, x3, z / length);
        }

    }

    public sealed class PointField5x5 : PointField
    {
        float[,] values;

        public PointField5x5(float x0z0, float x1z0, float x2z0, float x3z0, float x4z0,
                             float x0z1, float x1z1, float x2z1, float x3z1, float x4z1,
                             float x0z2, float x1z2, float x2z2, float x3z2, float x4z2,
                             float x0z3, float x1z3, float x2z3, float x3z3, float x4z3,
                             float x0z4, float x1z4, float x2z4, float x3z4, float x4z4)
        {
            values = new float[,] { {x0z0,x0z1,x0z2,x0z3,x0z4},
                                    {x1z0,x1z1,x1z2,x1z3,x1z4},
                                    {x2z0,x2z1,x2z2,x2z3,x2z4},
                                    {x3z0,x3z1,x3z2,x3z3,x3z4},
                                    {x4z0,x4z1,x4z2,x4z3,x4z4}};
        }

        public override float GetValue(float x, float z, float length, InterpolationMethod method)
        {
            float xT = (float)x / length;

            float x0 = SmootherSplineInterpolation(values[0, 0], values[1, 0], values[2, 0], values[3, 0], values[4, 0], xT);
            float x1 = SmootherSplineInterpolation(values[0, 1], values[1, 1], values[2, 1], values[3, 1], values[4, 1], xT);
            float x2 = SmootherSplineInterpolation(values[0, 2], values[1, 2], values[2, 2], values[3, 2], values[4, 2], xT);
            float x3 = SmootherSplineInterpolation(values[0, 3], values[1, 3], values[2, 3], values[3, 3], values[4, 3], xT);
            float x4 = SmootherSplineInterpolation(values[0, 4], values[1, 4], values[2, 4], values[3, 4], values[4, 4], xT);

            return SmootherSplineInterpolation(x0, x1, x2, x3, x4, (float)z / length);
            
        }

        public override float GetValue(float x, float z, InterpolationMethod method)
        {
            return GetValue(x, z, 5, method);
        }
    }
}
