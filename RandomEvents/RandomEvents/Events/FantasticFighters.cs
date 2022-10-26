﻿using System;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class FantasticFighters : BaseEvent
	{
		private const string EventTitle = "Fantastic Fighters?";
		
		private readonly int renownGain;

		public FantasticFighters() : base(Settings.ModSettings.RandomEvents.FantasticFightersData)
		{
			renownGain = Settings.ModSettings.RandomEvents.FantasticFightersData.renownGain;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return Hero.MainHero.Clan != null;
		}

		public override void StartEvent()
		{
			try
			{
				Hero.MainHero.Clan.Renown += renownGain;

				InformationManager.ShowInquiry(
					new InquiryData(EventTitle,
						"A rumor spreads that your clan managed to decisively win a battle when outnumbered 10-1.",
						true,
						false,
						"Done",
						null,
						null,
						null
						),
					true);

				StopEvent();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error while playing \"{randomEventData.eventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
		}

		private void StopEvent()
		{
			try
			{
				onEventCompleted.Invoke();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error while stopping \"{randomEventData.eventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
		}
	}

	public class FantasticFightersData : RandomEventData
	{
		public readonly int renownGain;

		public FantasticFightersData(string eventType, float chanceWeight, int renownGain) : base(eventType, chanceWeight)
		{
			this.renownGain = renownGain;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new FantasticFighters();
		}
	}
}
