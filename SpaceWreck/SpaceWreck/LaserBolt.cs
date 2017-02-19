using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceWreck
{
    public class LaserBolt
    {
        public static Texture2D Texture;
        public static Texture2D WeaponHit;
        public static float MaxRange = 600.0f;
        public static float Speed = 8.0f;
        public static int Damage = 1;

        public Vector2 Location { get; set; }
        public Vector2 Orientation { get; set; }
        public Vector2 SourceLocation { get; set; }
        public Vector2 Origin
        {
            get
            {
                return new Vector2(Location.X + 16, Location.Y + 16);
            }
        }
        public Rectangle BoundingRectangle
        {
            get
            {
                return new Rectangle(Convert.ToInt32(Location.X), Convert.ToInt32(Location.Y), 16, 16);
            }
        }
        public float DistanceTravelled { get; set; }
        public bool Decayed { get; set; }
        public bool Explosion { get; set; }

        public LaserBolt(Vector2 orientation, Vector2 location)
        {
            DistanceTravelled = 0.0f;
            Location = SourceLocation = location;
            Orientation = orientation;
            Decayed = false;
        }

        public void Update(List<Zombie> zombies)
        {
            if (Explosion)
            {
                Decayed = true;
                return;
            }
            var hit = zombies.Where(z => BoundingRectangle.Intersects(z.BoundingRectangle));
            foreach (var zombie in hit)
            {
                zombie.Health -= Damage;
                Explosion = true;
            }

            if (DistanceTravelled >= MaxRange)
            {
                Decayed = true;
            }

            if (!Decayed)
            {
                Location += new Vector2(Orientation.X, -Orientation.Y)*Speed;
                DistanceTravelled += Orientation.Length()*2;
            }
        }
    }
}
