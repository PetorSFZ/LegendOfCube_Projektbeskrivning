﻿using LegendOfCube.Engine.BoundingVolumes;
using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LegendOfCube.Engine
{
	/// <summary>
	/// Helper class to construct complex entities easily.
	/// </summary>
	public class EntityBuilder
	{
		private Properties properties = new Properties();
		private Vector3 velocity;
		private Vector3 acceleration;
		private Matrix transform = Matrix.Identity;
		private float maxAcceleration;
		private float maxSpeed;
		private Model model;
		private StandardEffectParams sep;
		private OBB modelSpaceOBB;

		/// <summary>
		/// Assign a model for the entity being built.
		/// </summary>
		/// <param name="model">The XNA 3D model</param>
		/// <returns>An instance of this, for chaining</returns>
		public EntityBuilder WithModel(Model model)
		{
			properties.Add(Properties.MODEL);
			this.model = model;
			return this;
		}

		/// <summary>
		/// Assigns a OBB BoundingVolume defined in model space for entity being built.
		/// </summary>
		/// <param name="modelSpaceOBB">The OBB defined in model space</param>
		/// <returns>An instance of this, for chaining</returns>
		public EntityBuilder WithBoundingVolume(OBB modelSpaceOBB)
		{
			properties.Add(Properties.MODEL_SPACE_BV);
			this.modelSpaceOBB = modelSpaceOBB;
			return this;
		}

		/// <summary>
		/// Assign a position for the entity being built. If WithTransform
		/// has been called, the translation data will be replaced, but the
		/// rest will remain unchanged.
		/// </summary>
		/// <param name="position">The position in world space</param>
		/// <returns>An instance of this, for chaining</returns>
		public EntityBuilder WithPosition(Vector3 position)
		{
			properties.Add(Properties.TRANSFORM);
			this.transform.Translation = position;
			return this;
		}

		/// <summary>
		/// Assign a transform matrix for the entity being built. This will override
		/// the effect of having prevously called WithPosition.
		/// </summary>
		/// <param name="transform">The model-to-world matrix</param>
		/// <returns>An instance of this, for chaining</returns>
		public EntityBuilder WithTransform(Matrix transform)
		{
			properties.Add(Properties.TRANSFORM);
			this.transform = transform;
			return this;
		}

		/// <summary>
		/// Assign a velocity for the entity being built.
		/// </summary>
		/// <param name="velocity">The initial velocity for the entity</param>
		/// <returns>An instance of this, for chaining</returns>
		public EntityBuilder WithVelocity(Vector3 velocity, float maxSpeed)
		{
			properties.Add(Properties.VELOCITY);
			this.velocity = velocity;
			this.maxSpeed = maxSpeed;
			return this;
		}

		/// <summary>
		/// Assign an acceleration for the entity being built.
		/// </summary>
		/// <param name="acceleration">The initial acceleration for the entity</param>
		/// <returns>An instance of this, for chaining</returns>
		public EntityBuilder WithAcceleration(Vector3 acceleration, float maxAcceleration)
		{
			properties.Add(Properties.ACCELERATION);
			this.acceleration = acceleration;
			this.maxAcceleration = maxAcceleration;
			return this;
		}

		public EntityBuilder WithStandardEffectParams(StandardEffectParams sep)
		{
			properties.Add(Properties.FULL_LIGHT_EFFECT);
			this.sep = sep;
			return this;
		}

		/// <summary>
		/// Add any property flags to the entity being created.
		/// </summary>
		/// <param name="properties">The properties to add</param>
		/// <returns>An instance of this, for chaining</returns>
		public EntityBuilder WithAdditionalProperties(Properties properties)
		{
			this.properties.Add(properties);
			return this;
		}

		/// <summary>
		/// Adds an Entity to the world, with the properties given to the builder.
		/// </summary>
		/// <param name="world">The world to add the entity to</param>
		/// <returns>A representation of the Entity</returns>
		public Entity AddToWorld(World world)
		{
			Entity entity = world.CreateEntity(properties);

			if (properties.Satisfies(Properties.TRANSFORM))
			{
				world.Transforms[entity.Id] = transform;
			}
			if (properties.Satisfies(Properties.ACCELERATION))
			{
				world.Accelerations[entity.Id] = acceleration;
				world.MaxAcceleration[entity.Id] = maxAcceleration;
			}
			if (properties.Satisfies(Properties.VELOCITY))
			{
				world.Velocities[entity.Id] = velocity;
				world.MaxSpeed[entity.Id] = maxSpeed;
			}
			if (properties.Satisfies(Properties.MODEL))
			{
				world.Models[entity.Id] = model;
			}
			if (properties.Satisfies(Properties.FULL_LIGHT_EFFECT))
			{
				world.StandardEffectParams[entity.Id] = sep;
			}
			if (properties.Satisfies(Properties.INPUT_FLAG))
			{
				// Not entirely sure if INPUT_FLAG implies having InputData
				// TODO: Rename INPUT_FLAG to INPUT as it implies data.
				world.InputData[entity.Id] = new InputDataImpl();
			}
			if (properties.Satisfies(Properties.MODEL_SPACE_BV))
			{
				world.ModelSpaceBVs[entity.Id] = this.modelSpaceOBB;
			}
			return entity;
		}
	}
}
