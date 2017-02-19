using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceWreck
{
    public static class Utility
    {
        public static Random Randomizer = new Random(DateTime.Now.Millisecond);

        public static float GetAngle(Vector2 vector)
        {
            if (vector.X > 0)
            {
                if (vector.Y < 0)
                {
                    return (3.0f / 4.0f * MathHelper.Pi);
                }
                if (vector.Y > 0)
                {
                    return (MathHelper.Pi / 4);
                }
                return MathHelper.Pi / 2;
            }
            if (vector.X < 0)
            {
                if (vector.Y < 0)
                {
                    return (MathHelper.Pi + MathHelper.Pi / 4);
                }
                if (vector.Y > 0)
                {
                    return (MathHelper.Pi + 3.0f / 4.0f * MathHelper.Pi);
                }
                return MathHelper.Pi + MathHelper.Pi / 2;
            }
            return vector.Y < 0 ? MathHelper.Pi : 0;
        }

        public static float VectorToAngle(Vector2 vector)
        {
            return (float)Math.Atan2(vector.X, vector.Y);
        }

        public static Vector2 AngleToVector(float angle)
        {
            return new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle));
        }

        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }


    }
}
