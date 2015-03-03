﻿using LegendOfCube.Engine;
using LegendOfCube.Engine.BoundingVolumes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LegendOfCube.Levels.Assets
{
	class Platform : Asset
	{

		protected override void loadAssets()
		{
			model = game.Content.Load<Model>("Models/Platform/platform");
			obb = new OBB(new Vector3(0, -.25f, 0), Vector3.UnitX, Vector3.UnitY,
				Vector3.UnitZ, new Vector3(10, .5f, 10));
		}
		public void Add(Vector3 position)
		{
			new EntityBuilder().WithModel(model)
				.WithPosition(position)
				.WithBoundingVolume(obb)
				.AddToWorld(world);
		}

		public void AddMoving(Vector3[] waypoints, float speed)
		{
			new EntityBuilder().WithModel(model)
				.WithPosition(waypoints[0])
				.WithVelocity(Vector3.UnitX * speed,0)
				.WithBoundingVolume(obb)
				.WithAI(waypoints, true)
				.AddToWorld(world);
		}

		public Platform(World world, Game game) :
			base(world, game)
		{
			loadAssets();
		}
	}
}