﻿using LegendOfCube.Engine;
using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Level
{
	class ConceptLevel
	{
		private static Entity playerEntity;
		private static Entity platform;
		

		public static void CreateLevel(World world, Game game)
		{
			var cubeModel = game.Content.Load<Model>("Models/Cube/cube_clean");
			var platformModel = game.Content.Load<Model>("Models/Platform/platform");

			var playerEffect = new StandardEffectParams
			{
				//DiffuseTexture = Game.Content.Load<Texture>("Models/Cube/cube_diff"),
				EmissiveTexture = game.Content.Load<Texture>("Models/Cube/cube_emissive"),
				SpecularColor = Color.Gray.ToVector4(),
				EmissiveColor = Color.White.ToVector4()
			};

			var otherCubeEffect = new StandardEffectParams
			{
				//DiffuseTexture = Game.Content.Load<Texture>("Models/Cube/cube_diff"),
				//SpecularTexture = Game.Content.Load<Texture>("Models/Cube/cube_specular"),
				//EmissiveTexture = Game.Content.Load<Texture>("Models/Cube/cube_emissive"),
				//NormalTexture = Game.Content.Load<Texture>("Models/Cube/cube_normal"),
				//SpecularColor = Color.White.ToVector4(),
				//EmissiveColor = Color.White.ToVector4()
			};

			platform =
				new EntityBuilder().WithModel(platformModel)
					//.WithTransform(Matrix.CreateScale(0.1f))
					.WithPosition(new Vector3(0, 0, 0))
					.WithBoundingVolume(new OBB(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(5, 5, 0.25f)))
					.AddToWorld(world);

			playerEntity =
				new EntityBuilder().WithModel(cubeModel)
					.WithPosition(new Vector3(0, 25.0f, 0))
					.WithVelocity(Vector3.Zero, 15)
					.WithAcceleration(Vector3.Zero, 30)
					.WithStandardEffectParams(playerEffect)
					.WithBoundingVolume(new OBB(new Vector3(0, 0.5f, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1), new Vector3(1, 1, 1)))
					.WithAdditionalProperties(new Properties(Properties.INPUT_FLAG | Properties.GRAVITY_FLAG))
					.AddToWorld(world);

			world.Player = playerEntity;
		}
	}
}