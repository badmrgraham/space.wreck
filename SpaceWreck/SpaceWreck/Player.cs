using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceWreck
{
    public class Player
    {
        public Texture2D TopSprite { get; set; }
        public Texture2D BottomSprite { get; set; }
        public Vector2 Location { get; set; }
        public Vector2 BottomOrientation { get; set; }
        public Vector2 TopOrientation { get; set; }
        public float BottomRotationSpeed { get; set; }
        public float TopRotationSpeed { get; set; }
        public float TopSpeed { get; set; }
        public float AccelerationRatio { get; set; }
        public float CurrentSpeed { get; set; }
        public List<LaserBolt> laserBolts { get; set; }
        public TimeSpan FiringGroup1Cooldown { get; set; }
        public int Health { get; set; }
        public Rectangle BoundingRectangle
        {
            get
            {
                var hitsize = 16;
                return new Rectangle(Convert.ToInt32(Location.X + hitsize / 2), Convert.ToInt32(Location.Y + hitsize / 2), 16, 16);
            }
        }
        public Vector2 Origin
        {
            get
            {
                return new Vector2(Location.X + 16, Location.Y + 16);
            }
        }

        public Player()
        {
            BottomRotationSpeed = 0.1f;
            TopRotationSpeed = 0.1f;
            TopSpeed = 8.0f;
            AccelerationRatio = 0.1f;
            laserBolts = new List<LaserBolt>();
            Health = 3;
        }

        public void Update(GameTime gameTime, List<Zombie> zombies)
        {
            if (Health <= 0)
            {
                return;
            }
            // adjust angle
            if (InputManager.Thrust != Vector2.Zero)
            {
                BottomOrientation = AdjustThrust(InputManager.Thrust);
            }
            if (InputManager.FiringAngle != Vector2.Zero)
            {
                TopOrientation = AdjustFiringAngle(InputManager.FiringAngle);
            }

            if (zombies.Any(z => z.BoundingRectangle.Intersects(BoundingRectangle)))
            {
                Health -= 1;
            }

            if (InputManager.FiringWeaponGroup1 && FiringGroup1Cooldown < gameTime.TotalGameTime)
            {
                FiringGroup1Cooldown = gameTime.TotalGameTime + new TimeSpan(0, 0, 0, 0, 200);
                laserBolts.Add(new LaserBolt(TopOrientation, Location));
            }

//             move location
            if (InputManager.Thrust.X != 0 || InputManager.Thrust.Y != 0)
            {
                Location = new Vector2(Location.X + BottomOrientation.X, Location.Y + -BottomOrientation.Y);
            }
        }

        private Vector2 AdjustFiringAngle(Vector2 newFiringAngle)
        {
            var topAngle = Utility.VectorToAngle(TopOrientation);
            var topThrust = Utility.VectorToAngle(newFiringAngle);

            var oppositeAngle = 0.0f;
            if (topAngle > 0)
            {
                oppositeAngle = (float)(topAngle - Math.PI);
            }
            else if (topAngle < 0)
            {
                oppositeAngle = (float)(topAngle + Math.PI);
            }

            var rotateClockwise = true;
            if (topAngle > Math.PI / 2 || topAngle >= 0)
            {
                rotateClockwise = topThrust > topAngle || topThrust < oppositeAngle;
            }
            else if (topAngle < -Math.PI / 2 || topAngle < 0)
            {
                rotateClockwise = topThrust > topAngle && topThrust < oppositeAngle;
            }

            // these top 2 are edge cases where it goes from -3.14 to 3.14
            if (rotateClockwise && topAngle + TopRotationSpeed > Math.PI)
            {
                var extended = topAngle + TopRotationSpeed - 2 * Math.PI;
                if (topThrust <= topAngle || topThrust >= extended)
                {
                    topAngle = (float)extended;
                }
                else
                {
                    topAngle = topThrust;
                }
            }
            else if (!rotateClockwise && topAngle - TopRotationSpeed < -Math.PI)
            {
                var extended = topAngle - TopRotationSpeed + 2 * Math.PI;
                if (topThrust >= topAngle || topThrust >= extended)
                {
                    topAngle = (float)extended;
                }
                else
                {
                    topAngle = topThrust;
                }
            }
            // these are the core rotations... gotta find out which rotation is quicker.
            else
            {
                if (rotateClockwise)
                {
                    if (topThrust >= topAngle && topThrust <= topAngle + TopRotationSpeed)
                    {
                        topAngle = topThrust;
                    }
                    else
                    {
                        topAngle += TopRotationSpeed;
                        if (topAngle > Math.PI)
                        {
                            topAngle -= (float)(2*Math.PI);
                        }
                    }
                }
                else
                {
                    if (topThrust <= topAngle && topThrust >= topAngle - TopRotationSpeed)
                    {
                        topAngle = topThrust;
                    }
                    else
                    {
                        topAngle -= TopRotationSpeed;
                        if (topAngle < Math.PI)
                        {
                            topAngle += (float)(2 * Math.PI);
                        }
                    }
                }
            }
            return Utility.AngleToVector(topAngle);
        }

        private Vector2 AdjustThrust(Vector2 newThrust)
        {
            var bottomAngle = Utility.VectorToAngle(BottomOrientation);
            var bottomThrust = Utility.VectorToAngle(newThrust);

            var oppositeAngle = 0.0f;
            if (bottomAngle > 0)
            {
                oppositeAngle = (float)(bottomAngle - Math.PI);
            }
            else if (bottomAngle < 0)
            {
                oppositeAngle = (float)(bottomAngle + Math.PI);
            }

            var rotateClockwise = true;
            if (bottomAngle > Math.PI / 2 || bottomAngle >= 0)
            {
                rotateClockwise = bottomThrust > bottomAngle || bottomThrust < oppositeAngle;
            }
            else if (bottomAngle < -Math.PI / 2 || bottomAngle < 0)
            {
                rotateClockwise = bottomThrust > bottomAngle && bottomThrust < oppositeAngle;
            }

            // these top 2 are edge cases where it goes from -3.14 to 3.14
            if (rotateClockwise && bottomAngle + BottomRotationSpeed > Math.PI)
            {
                var extended = bottomAngle + BottomRotationSpeed - 2 * Math.PI;
                if (bottomThrust <= bottomAngle || bottomThrust >= extended)
                {
                    bottomAngle = (float)extended;
                }
                else
                {
                    bottomAngle = bottomThrust;
                }
            }
            else if (!rotateClockwise && bottomAngle - BottomRotationSpeed < -Math.PI)
            {
                var extended = bottomAngle - BottomRotationSpeed + 2 * Math.PI;
                if (bottomThrust >= bottomAngle || bottomThrust >= extended)
                {
                    bottomAngle = (float)extended;
                }
                else
                {
                    bottomAngle = bottomThrust;
                }
            }
            // these are the core rotations... gotta find out which rotation is quicker.
            else
            {
                if (rotateClockwise)
                {
                    if (bottomThrust >= bottomAngle && bottomThrust <= bottomAngle + BottomRotationSpeed)
                    {
                        bottomAngle = bottomThrust;
                    }
                    else
                    {
                        bottomAngle += BottomRotationSpeed;
                        if (bottomAngle > Math.PI)
                        {
                            bottomAngle -= (float)(2 * Math.PI);
                        }
                    }
                }
                else
                {
                    if (bottomThrust <= bottomAngle && bottomThrust >= bottomAngle - BottomRotationSpeed)
                    {
                        bottomAngle = bottomThrust;
                    }
                    else
                    {
                        bottomAngle -= BottomRotationSpeed;
                        if (bottomAngle < Math.PI)
                        {
                            bottomAngle += (float)(2 * Math.PI);
                        }
                    }
                }
            }
            return Utility.AngleToVector(bottomAngle);

//            var bottomAngle = Utility.VectorToAngle(BottomOrientation);
//            var bottomThrust = Utility.VectorToAngle(newThrust);
//
//            if (bottomAngle + BottomRotationSpeed > Math.PI)
//            {
//                var extended = bottomAngle + BottomRotationSpeed - 2*Math.PI;
//                if (bottomThrust > bottomAngle || (bottomThrust < extended && bottomThrust > -Math.PI))
//                {
//                    bottomAngle = (float)extended;
//                }
//                else
//                {
//                    bottomAngle = bottomThrust;
//                }
//            }
//            else if (bottomAngle - BottomRotationSpeed < -Math.PI)
//            {
//                var extended = bottomAngle - BottomRotationSpeed + 2*Math.PI;
//                if (bottomThrust < bottomAngle || (bottomThrust > extended && bottomThrust < Math.PI))
//                {
//                    bottomAngle = (float) extended;
//                }
//                else
//                {
//                    bottomAngle = bottomThrust;
//                }
//            }
//            else if (bottomThrust > bottomAngle)
//            {
//                if (bottomAngle + BottomRotationSpeed > bottomThrust)
//                {
//                    bottomAngle = bottomThrust;
//                }
//                else
//                {
//                    bottomAngle += BottomRotationSpeed;
//                }
//            }
//            else
//            {
//                if (bottomAngle - BottomRotationSpeed < bottomThrust)
//                {
//                    bottomAngle = bottomThrust;
//                }
//                else
//                {
//                    bottomAngle -= BottomRotationSpeed;
//                }
//            }
//            return Utility.AngleToVector(bottomAngle);
        }

        private float CurveAngle(float from, float to, float step)
        {
            if (step == 0) return from;
            if (from == to || step == 1) return to;

            var fromVector = new Vector2((float)Math.Cos(from), (float)Math.Sin(from));
            var toVector = new Vector2((float)Math.Cos(to), (float)Math.Sin(to));

            var currentVector = Slerp(fromVector, toVector, step);

            return (float)Math.Atan2(currentVector.Y, currentVector.X);
        }

        private static Vector2 Slerp(Vector2 from, Vector2 to, float step)
        {
            if (step == 0) return from;
            if (from == to || step == 1) return to;

            var theta = Math.Acos(Vector2.Dot(from, to));
            if (theta == 0) return to;

            var sinTheta = Math.Sin(theta);
            return (float)(Math.Sin((1 - step) * theta) / sinTheta) * from + (float)(Math.Sin(step * theta) / sinTheta) * to;
        }
    }
}
