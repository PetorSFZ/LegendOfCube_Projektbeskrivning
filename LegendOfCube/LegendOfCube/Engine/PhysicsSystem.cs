﻿using System;
using Microsoft.Xna.Framework;
using LegendOfCube.Engine.BoundingVolumes;

namespace LegendOfCube.Engine
{
	public class PhysicsSystem
	{
		// Constants
		// * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

		private static readonly Properties ACCELERATABLE = new Properties(
		                                                               Properties.VELOCITY |
		                                                               Properties.ACCELERATION);

		private static readonly Properties HAS_GRAVITY = new Properties(
		                                                         Properties.VELOCITY |
		                                                         Properties.GRAVITY_FLAG);

		private static readonly Properties MOVABLE = new Properties(
		                                                         Properties.TRANSFORM |
		                                                         Properties.VELOCITY);

		private static readonly Properties HAS_FRICTION = new Properties(
		                                                 Properties.TRANSFORM |
		                                                 Properties.VELOCITY |
		                                                 Properties.FRICTION_FLAG);

		private static readonly Properties HAS_BV = new Properties(Properties.BOUNDING_VOLUME);

		public void ApplyPhysics(float delta, World world)
		{
			for (UInt32 i = 0; i < world.MaxNumEntities; i++)
			{
				Properties properties = world.EntityProperties[i];
				// Check if velocity should be updated
				if (properties.Satisfies(ACCELERATABLE))
				{
					world.Velocities[i] += (world.Accelerations[i] * delta);

					// Clamp velocity in X and Y direction
					Vector2 groundVelocity = new Vector2(world.Velocities[i].X, world.Velocities[i].Z);
					if (groundVelocity.Length() > world.MaxSpeed[i])
					{
						groundVelocity.Normalize();
						groundVelocity *= world.MaxSpeed[i];
						world.Velocities[i].X = groundVelocity.X;
						world.Velocities[i].Z = groundVelocity.Y;
					}
				}

				// Apply gravity
				if (properties.Satisfies(HAS_GRAVITY))
				{
					world.Velocities[i] += (world.Gravity * delta);
				}

				// Update position
				if (properties.Satisfies(MOVABLE))
				{
					if (properties.Satisfies(HAS_BV))
					{
						Vector3 newPosition = world.Transforms[i].Translation + (world.Velocities[i] * delta);
						OBB newBV = world.BVs[i];
						newBV.Position = newPosition;

						// Check if collision
						UInt32 crashIndex = UInt32.MaxValue;
						for (UInt32 bvIndex = 0; bvIndex < world.MaxNumEntities; bvIndex++)
						{
							if (!world.EntityProperties[bvIndex].Satisfies(HAS_BV)) continue;
							if (IntersectionsTests.Intersects(ref newBV, ref world.BVs[bvIndex]))
							{
								crashIndex = bvIndex;
								break;
							}
						}

						// No collision
						if (crashIndex == UInt32.MaxValue)
						{
							world.Transforms[i].Translation = newPosition;
							world.BVs[i] = newBV;
						}
						else // collision
						{
							// Do nothing.
						}

						// Hacky floor
						if (world.Transforms[i].Translation.Y < 0.5f)
						{
							Vector3 translation = world.Transforms[i].Translation;
							translation.Y = 0.0f;
							world.Transforms[i].Translation = translation;
							world.Velocities[i].Y = 0.0f;
							//Reset air state
							world.PlayerCubeState.InAir = false;

							world.BVs[i].Position = world.Transforms[i].Translation;
						}
					}
					else
					{
						world.Transforms[i].Translation += (world.Velocities[i] * delta);
						// Hacky floor
						if (world.Transforms[i].Translation.Y < 0)
						{
							Vector3 translation = world.Transforms[i].Translation;
							translation.Y = 0.0f;
							world.Transforms[i].Translation = translation;
							world.Velocities[i].Y = 0.0f;
							//Reset air state
							world.PlayerCubeState.InAir = false;
						}
					}
				}
			}
		}
	}
}
