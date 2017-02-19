using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SpaceWreck
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private List<Zombie> zombies;
        private TimeSpan timeToSpawnNewZombie = TimeSpan.MinValue;
        public static int Width { get; set; }
        public static int Height { get; set; }

        public Player player { get; set; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new Player
                         {
                             Location = new Vector2(32, 32)
                         };

            zombies = new List<Zombie>();
            zombies.Add(new Zombie(new Vector2(300, 300), new Vector2(0,0)));

            Width = GraphicsDevice.Viewport.Bounds.Width;
            Height = GraphicsDevice.Viewport.Bounds.Height;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player.TopSprite = Content.Load<Texture2D>("Sprites/Mech/top1");
            player.BottomSprite = Content.Load<Texture2D>("Sprites/Mech/bottom1");
            LaserBolt.Texture = Content.Load<Texture2D>("Sprites/Projectile/LaserBolt");
            LaserBolt.WeaponHit = Content.Load<Texture2D>("Sprites/Projectile/LaserBoltHit");
            Zombie.Texture = Content.Load<Texture2D>("Sprites/Enemy/Zombie");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            InputManager.HandleInput();

            if (InputManager.Shutdown)
            {
                Exit();
            }

            if (timeToSpawnNewZombie < gameTime.TotalGameTime)
            {
                zombies.Add(Zombie.CreateRandomZombie());
                timeToSpawnNewZombie = gameTime.TotalGameTime + new TimeSpan(0, 0, 3);
            }

            foreach (var laser in player.laserBolts)
            {
                laser.Update(zombies);
            }
            player.laserBolts.RemoveAll(laser => laser.Decayed);

            foreach (var zombie in zombies)
            {
                zombie.Update(player);
            }
            zombies.RemoveAll(z => z.Health <= 0);

            player.Update(gameTime, zombies);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGoldenrod);
            spriteBatch.Begin();

            if (player.Health >= 0)
            {
                spriteBatch.Draw(player.BottomSprite,
                                 new Rectangle(Convert.ToInt32(player.Location.X), Convert.ToInt32(player.Location.Y),
                                               32, 32),
                                 new Rectangle(0, 0, 32, 32), Color.White,
                                 Utility.VectorToAngle(player.BottomOrientation), new Vector2(16, 16),
                                 SpriteEffects.None, 0.0f);

                spriteBatch.Draw(player.TopSprite,
                                 new Rectangle(Convert.ToInt32(player.Location.X), Convert.ToInt32(player.Location.Y),
                                               32, 32),
                                 new Rectangle(0, 0, 32, 32), Color.White, Utility.VectorToAngle(player.TopOrientation),
                                 new Vector2(16, 16), SpriteEffects.None, 0.0f);
            }

            foreach(var laser in player.laserBolts)
            {
                var laserLocation = Vector2.Transform(new Vector2(-10, -12),
                                      Matrix.CreateRotationZ(Utility.VectorToAngle(laser.Orientation))) + laser.Location;

                var texture = laser.Explosion ? LaserBolt.WeaponHit : LaserBolt.Texture;

                spriteBatch.Draw(texture, new Rectangle(Convert.ToInt32(laserLocation.X), Convert.ToInt32(laserLocation.Y), 16, 16),
                    new Rectangle(0, 0, 16, 16), Color.White, Utility.VectorToAngle(laser.Orientation), new Vector2(8,8), SpriteEffects.None, 0.0f);
            }

            foreach (var zombie in zombies)
            {
                zombie.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
