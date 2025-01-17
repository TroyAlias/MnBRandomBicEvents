﻿using System;
using System.Windows;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.Engine;
using TaleWorlds.CampaignSystem;
using CryingBuffalo.RandomEvents.Settings.MCM;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class DreadedSweats : BaseEvent
	{
		private readonly int minMoraleLoss;
		private readonly int maxMoraleLoss;
		private readonly int minvictim;
        private readonly int maxvictim;





		public DreadedSweats() : base(Settings.ModSettings.RandomEvents.DreadedSweatsData)
		{
			minMoraleLoss = MCM_MenuConfig_A_M.Instance.DS_minMoraleLoss;
			maxMoraleLoss = MCM_MenuConfig_A_M.Instance.DS_maxMoraleLoss;
			minvictim = MCM_MenuConfig_A_M.Instance.DS_minvictim;
			maxvictim = MCM_MenuConfig_A_M.Instance.DS_maxvictim;

		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MCM_MenuConfig_A_M.Instance.DS_Disable == false && MobileParty.MainParty.MemberRoster.TotalHealthyCount >= 10;
		}

		public override void StartEvent()
		{

      		var partysize = MobileParty.MainParty.MemberRoster.TotalHealthyCount;
			var moraleLoss = MBRandom.RandomInt(minMoraleLoss, maxMoraleLoss);
			var victims = MBRandom.RandomInt(minvictim, maxvictim);
			var totalvictims = partysize / 10 + victims;
			Hero.MainHero.AddSkillXp(DefaultSkills.Medicine, 5);
			MobileParty.MainParty.SetIsDisorganized(true);
			MobileParty.MainParty.RecentEventsMorale -= moraleLoss;
			MobileParty.MainParty.MoraleExplained.Add(-moraleLoss);
			MobileParty.MainParty.MemberRoster.WoundNumberOfTroopsRandomly(totalvictims);

			
				var eventTitle2 = new TextObject("{=DreadedSweats_Title}The Dreaded Sweat").ToString();

				var eventOption2 = new TextObject("{=DreadedSweats_Event_Text}There has been an outbreak of some sort of illness among the troops.  Fever, debilitating aches and pains, a few of the men can barely stand.  " +
					"All the symptoms point towards this being what many call 'the Dreaded Sweats'.  You order your men to wash up in the creak before moving out, hopefully this won't become something too serious.")
					.ToString();

				var eventButtonText = new TextObject("{=DreadedSweats_Event_Button_Text}Done").ToString();

				InformationManager.ShowInquiry(new InquiryData(eventTitle2, eventOption2, true, false, eventButtonText, null, null, null), true);

				string eventMsg1 = new TextObject(
				"{=DreadedSweats_Event_Msg_1}{totalvictims} troops have the Dreaded Sweats!")
				.SetTextVariable("totalvictims", totalvictims)
				.ToString();
			InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_POS_Outcome));
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

	public class DreadedSweatsData : RandomEventData
	{
		public readonly int minMoraleLoss;
		public readonly int maxMoraleLoss;
		public readonly int minvictim;
		public readonly int maxvictim;


		public DreadedSweatsData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{




	}

		public override BaseEvent GetBaseEvent()
		{
			return new DreadedSweats();
		}
	}
}
