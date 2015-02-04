﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine
{
	public class RenderSystem
	{
		// Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
        private static readonly Properties TRANSFORM = new Properties(Properties.TRANSFORM);
		private static readonly Properties MODEL_AND_TRANSFORM = new Properties(
		                                                                Properties.MODEL |
		                                                                Properties.TRANSFORM);

		// Members
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private Game game;
		private GraphicsDeviceManager graphics;

		// Constructors
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public RenderSystem(Game game)
		{
			this.game = game;
			graphics = new GraphicsDeviceManager(game);
		}

		// Public methods
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		public void Initialize()
		{
			game.Window.AllowUserResizing = true;
			graphics.PreferMultiSampling = true;
			game.GraphicsDevice.BlendState = BlendState.Opaque;
			game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
			graphics.ApplyChanges();
		}

		public void DrawEntities(World world)
		{
			Vector3 playerPos = new Vector3();
			//Find player
			for (UInt32 i = 0; i < world.MaxNumEntities; i++)
			{
				if (world.EntityProperties[i].Satisfies(new Properties(Properties.INPUT_FLAG)))
				{
					playerPos = world.Transforms[i].Translation;
					break;
				}
			}

			Vector3 camPos = playerPos;
			camPos.Y = 4;
			camPos.Z += 5;
			Vector3 camTarget = playerPos;
			Vector3 up = new Vector3(0, 1, 0);
			float fov = 75;

			Matrix view = Matrix.CreateLookAt(camPos, camTarget, up);
			Matrix projection = Matrix.CreatePerspectiveFieldOfView(
			                        MathHelper.ToRadians(fov),
			                        game.GraphicsDevice.Viewport.AspectRatio,
			                        0.1f,
			                        1000.0f);

			for (UInt32 i = 0; i < world.MaxNumEntities; i++) {
				if (world.EntityProperties[i].Satisfies(MODEL_AND_TRANSFORM)) {
					world.Models[i].Draw(world.Transforms[i], view, projection);
				}
			}
		}
	}
}
