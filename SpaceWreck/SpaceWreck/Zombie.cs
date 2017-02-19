using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceWreck
{
    public class Zombie
    {
        public static Texture2D Texture { get; set; }

        public Vector2 Location { get; set; }
        public Vector2 Orientation { get; set; }
        public float Speed { get; set; }
        public int Health { get; set; }
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
                var hitsize = 16;
                return new Rectangle(Convert.ToInt32(Location.X + hitsize / 2), Convert.ToInt32(Location.Y + hitsize / 2), 16, 16);
            }
        }

        public Zombie(Vector2 location, Vector2 orientation)
        {
            Location = location;
            Orientation = orientation;
            Health = 5;
            Speed = 0.3f;
        }

        public void Update(Player player)
        {
            var path = (player.Origin - Origin);
            path.Normalize();
            Orientation = path;
            Location += path*Speed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture,
                                new Rectangle(Convert.ToInt32(Location.X), Convert.ToInt32(Location.Y), 32, 32),
                                new Rectangle(0, 0, 32, 32), Color.White, Utility.VectorToAngle(Orientation),
                                new Vector2(16, 16), SpriteEffects.None, 0.0f);
        }

        public static Zombie CreateRandomZombie()
        {
            Vector2 location;
            switch (Utility.Randomizer.Next(0, 3))
            {
                case 0:
                    location = new Vector2(-32, Utility.Randomizer.Next(0, Game1.Height));
                    break;
                case 1:
                    location = new Vector2(Utility.Randomizer.Next(Game1.Width), -32);
                    break;
                case 2:
                    location = new Vector2(Game1.Width + 32, Utility.Randomizer.Next(0, Game1.Height));
                    break;
                default:
                    location = new Vector2(Utility.Randomizer.Next(0, Game1.Width), Game1.Height + 32);
                    break;
            }
            var orientation = new Vector2(0, 0);
            return new Zombie(location, orientation);
        }
    }
}
