﻿using System;

namespace Assets.Scripts.DataManagers
{

	public enum GuideStep
	{
		Welcome = 1, //开始
		ProduceGold,
		BuyWood,
		Build,
		Produce
	}

	public class GuideManager:Tools.XSingleton<GuideManager>
	{
		
		public GuideManager ()
		{
			Step = GuideStep.Welcome;
		}


		public GuideStep Step {
			set {
				GamePlayerManager.Singleton.GuideStep = (int)value;
			} 
			get {
				int step = GamePlayerManager.Singleton.GuideStep;
				if (step == -1)
					return GuideStep.Welcome;
				else
					return (GuideStep)step;
			}
		}



		public void EventSystem (GuideStep step)
		{
			switch (step) {
			case GuideStep.Welcome:
				
				break;
			}
		}

		public void StepTo (GuideStep step)
		{
			this.Step = step;
		}

	}
}

