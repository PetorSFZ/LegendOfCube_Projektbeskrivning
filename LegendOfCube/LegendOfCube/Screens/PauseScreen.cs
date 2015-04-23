﻿using LegendOfCube.Engine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LegendOfCube.Engine;
using LegendOfCube.Engine.Input;

namespace LegendOfCube.Screens
{
	class PauseScreen : MenuScreen
	{
		public PauseScreen(Game game, ScreenSystem screenSystem, InputHelper inputHelper) : base(game, screenSystem, inputHelper) {}

		internal override void LoadContent()
		{
			base.LoadContent();
			AddItemBelow("Return to Game", () =>
				ScreenSystem.RemoveCurrentScreen()
			);
			AddItemBelow("Main Menu", () =>
			{
				ScreenSystem.SetScreen(new StartScreen(Game, ScreenSystem, InputHelper));
			});
			AddItemBelow("Exit Game", () =>
				Game.Exit()
			);
		}
	}
}
