using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceWreck
{
    public static class InputManager
    {
        public static bool Shutdown { get; set; }
        public static Vector2 Thrust { get; set; }
        public static Vector2 FiringAngle { get; set; }
        public static bool FiringWeaponGroup1 { get; set; }

        public static void HandleGamePadInput()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Shutdown = true;
            }

            FiringWeaponGroup1 = GamePad.GetState(PlayerIndex.One).Triggers.Right > 0;

            Thrust = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
            FiringAngle = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right;
        }

        public static void HandleInput()
        {
            HandleGamePadInput();
        }
    }
}
