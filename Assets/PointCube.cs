using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public abstract class PointCube
    {
        public abstract float GetValue(float x, float y, float z, float length);
        public abstract float GetValue(float x, float y, float z);
        public static float LinearInterpolation(float f1, float f2, float t)
        {
            return (f1 * (1f - t)) + (f2 * t);
        }

        public static float SmoothInterpolation(float f1, float f2, float t)
        {
            return Mathf.SmoothStep(f1, f2, t);
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

        public static float SmootherSplineInterpolation(float f1, float f2, float f3, float t)
        {
            float s1 = SmoothInterpolation(f1, f2, t);
            float s2 = SmoothInterpolation(f2, f3, t);

            return SmoothInterpolation(s1, s2, t);
        }
    }

    public class PointCube2x2x2 : PointCube
    {
        PointField2x2[] pFields;

        public PointCube2x2x2(PointField2x2 y0, PointField2x2 y1)
        {
            pFields = new PointField2x2[] { y0, y1 };
        }

        public override float GetValue(float x, float y, float z, float length)
        {
            float tY = (float)y / length;
            return LinearInterpolation(pFields[0].GetValue(x,z,length,InterpolationMethod.Linear), 
                                       pFields[1].GetValue(x, z,length, InterpolationMethod.Linear), tY);
        }

        public override float GetValue(float x, float y, float z)
        {
            return GetValue(x, y, z, 2f);
        }
    }

    public class PointCube3x3x3 : PointCube
    {
        PointField3x3[] pFields;

        public PointCube3x3x3(PointField3x3 y0, PointField3x3 y1, PointField3x3 y2)
        {
            pFields = new PointField3x3[] { y0, y1, y2 };
        }

        public override float GetValue(float x, float y, float z, float length)
        {
            float tY = y / length;
            float y0 = pFields[0].GetValue(x, z, InterpolationMethod.Spline);
            float y1 = pFields[1].GetValue(x, z, InterpolationMethod.Spline);
            float y2 = pFields[2].GetValue(x, z, InterpolationMethod.Spline);

            return SmootherSplineInterpolation(y0, y1, y2, tY);
        }

        public override float GetValue(float x, float y, float z)
        {
            return GetValue(x, y, z, 3);
        }
    }
}
