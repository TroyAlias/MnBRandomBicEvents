﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class GranaryRats : BaseEvent
	{
		private const string EventTitle = "Rats in the granary!";
		
		private readonly float foodLossPercent;

		public GranaryRats() : base(Settings.ModSettings.RandomEvents.GranaryRatsData)
		{
			foodLossPercent = Settings.ModSettings.RandomEvents.GranaryRatsData.foodLossPercent;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return Hero.MainHero.Clan.Settlements.Any();
		}

		public override void StartEvent()
		{
			try
			{
				// The list of settlements that are able to have food added to them
				List<Settlement> eligibleSettlements = Hero.MainHero.Clan.Settlements.Where(s => s.IsTown || s.IsCastle).ToList();

				// Out of the settlements the main hero owns, only the towns or castles have food.

				// Randomly pick one of the eligible settlements
				int index = MBRandom.RandomInt(0, eligibleSettlements.Count);

				// Grab the winning settlement and add food to it
				Settlement infestedSettlement = eligibleSettlements[index];

				infestedSettlement.Town.FoodStocks -= MathF.Abs(infestedSettlement.Town.FoodChange * foodLossPercent);

				// set the name to display
				var ratSettlement = infestedSettlement.Name.ToString();

				InformationManager.ShowInquiry(
					new InquiryData(EventTitle,
									$"You have been informed that {ratSettlement} had an infestation of rats that went unchecked... The rats won't starve this month, but your peasants might.",
									true,
									false,
									"Done",
									null,
									null,
									null
									), true);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error while running \"{randomEventData.eventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}

			StopEvent();
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

	public class GranaryRatsData : RandomEventData
	{
		public readonly float foodLossPercent;

		public GranaryRatsData(string eventType, float chanceWeight, float foodLossPercent) : base(eventType, chanceWeight)
		{
			this.foodLossPercent = foodLossPercent;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new GranaryRats();
		}
	}
}
