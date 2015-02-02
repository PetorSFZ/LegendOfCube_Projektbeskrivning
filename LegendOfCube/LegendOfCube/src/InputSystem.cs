﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LegendOfCube
{
	public class InputSystem
	{
		// Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public static readonly ComponentMask MOVEMENT_INPUT = new ComponentMask(ComponentMask.TRANSFORM |
		                                                                        ComponentMask.RECEIVE_INPUT);

		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private Game game;
		private KeyboardState oldKeyState;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public InputSystem(Game game)
		{
			this.game = game;
			oldKeyState = Keyboard.GetState();
		}

		// Public methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public void ApplyInput(GameTime gameTime, World world)
		{
			KeyboardState keyState = Keyboard.GetState();

			for (UInt32 i = 0; i < world.MaxNumEntities; i++) {
				if (!world.ComponentMasks[i].Satisfies(MOVEMENT_INPUT)) continue;

				if (keyState.IsKeyDown(Keys.W))
				{
					world.Transforms[i] = Matrix.CreateTranslation(0.5f * world.Transforms[i].Forward) * world.Transforms[i];
				}

				/*if (keyState.IsKeyDown(Keys.W))
				{
					cube.ModelToWorld = Matrix.CreateTranslation(0.1f * cube.ModelToWorld.Forward) * cube.ModelToWorld;
				}
				if (keyState.IsKeyDown(Keys.A))
				{
					cube.ModelToWorld = Matrix.CreateTranslation(0.1f * cube.ModelToWorld.Left) * cube.ModelToWorld;
				}
				if (keyState.IsKeyDown(Keys.S))
				{
					cube.ModelToWorld = Matrix.CreateTranslation(0.1f * cube.ModelToWorld.Backward) * cube.ModelToWorld;
				}
				if (keyState.IsKeyDown(Keys.D))
				{
					cube.ModelToWorld = Matrix.CreateTranslation(0.1f * cube.ModelToWorld.Right) * cube.ModelToWorld;
				}
				if (keyState.IsKeyDown(Keys.Space) && !oldKeyState.IsKeyDown(Keys.Space))
				{
					if (cube.ModelToWorld.Translation.Y == 0)
					{
						cube.Vel.Y += 0.21f;
					}
					else if (cube.ModelToWorld.Translation.Y > 0 && doubleJump)
					{
						cube.Vel.Y += 0.21f;
						doubleJump = false;
					}
				}*/
			}

			oldKeyState = keyState;
		}
	}
}
