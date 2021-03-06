﻿using System;
using LegendOfCube.Engine;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace LegendOfCube.Levels
{
	class StairwayLevel : Level
	{
		public StairwayLevel() : base("Test: Stairway Level") {}

		public override World CreateWorld(Game game, ContentCollection contentCollection)
		{
			World world = new World(20000)
			{
				SpawnPoint = new Vector3(0, 0, 0),
				InitialViewDirection = Vector3.Normalize(new Vector3(1, 0, 0)),
				DirLight = new DirLight(Vector3.Normalize(new Vector3(1, -0.3f, 1))),
			};

			var player = new EntityBuilder()
				.WithModelData(contentCollection.PlayerCube2)
				.WithPosition(world.SpawnPoint)
				.WithVelocity(Vector3.Zero, 0)
				.WithAcceleration(Vector3.Zero)
				.WithAdditionalProperties(new Properties(Properties.INPUT | Properties.GRAVITY_FLAG | Properties.DYNAMIC_VELOCITY_FLAG))
				.AddToWorld(world);
			world.Player = player;

			var groundEffect = new StandardEffectParams
			{
				DiffuseColor = Color.GreenYellow.ToVector4()
			};

			// Add ground
			new EntityBuilder()
				.WithModelData(contentCollection.PlainCube)
				.WithStandardEffectParams(groundEffect)
				.WithTransform(Matrix.CreateTranslation(0.0f, -0.5f, 0.0f) * Matrix.CreateScale(5000.0f, 1.0f, 5000.0f))
				.AddToWorld(world);

			var stepBuilder1 = new EntityBuilder()
				.WithModelData(contentCollection.PlayerCubePlain)
				.WithStandardEffectParams(new StandardEffectParams { DiffuseColor = Color.BlueViolet.ToVector4() })
				.WithTransform(Matrix.CreateScale(3.0f, 0.5f, 100.0f));

			var stepBuilder2 = stepBuilder1.Copy()
				.WithStandardEffectParams(new StandardEffectParams { DiffuseColor = Color.AliceBlue.ToVector4() })
				.WithTransform(Matrix.CreateScale(3.0f, 0.5f, 100.0f));

			for (int i = 1; i <= 2000; i++)
			{
				float x = i * 3.0f;
				float y = (float)Math.Pow(i, 1.2) * 0.075f;
				var stairBuilder = i % 2 == 0 ? stepBuilder1 : stepBuilder2;

				if (i == 2000)
				{
					stairBuilder.Copy()
						.WithTransform(Matrix.CreateScale(3.0f, 0.5f, 100*(float) Math.Pow(i, -0.15f)))
						.WithPosition(new Vector3(x, y, 0))
						.WithAdditionalProperties(new Properties(Properties.WIN_ZONE_FLAG))
						.AddToWorld(world);
				}
				else
				{
					stairBuilder.Copy()
						.WithTransform(Matrix.CreateScale(3.0f, 0.5f, 100*(float) Math.Pow(i, -0.15f)))
						.WithPosition(new Vector3(x, y, 0))
						.AddToWorld(world);
				}
			}

			return world;
		}
	}
}
