﻿using System;
using System.Collections.Generic;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Inventory;
using CryingBuffalo.RandomEvents.Helpers;
using System.Linq;
using TaleWorlds.CampaignSystem.Roster;
using CryingBuffalo.RandomEvents.Settings.MCM;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class TravellingMerchant : BaseEvent
	{
		private readonly int minloot;
		private readonly int maxloot;

		public TravellingMerchant() : base(Settings.ModSettings.RandomEvents.TravellingMerchantData)
		{
			minloot = MCM_MenuConfig_N_Z.Instance.TM_minloot;
			maxloot = MCM_MenuConfig_N_Z.Instance.TM_maxloot;

		}
		public override void CancelEvent()
		{
		}
		public override bool CanExecuteEvent()
		{
			return MCM_MenuConfig_N_Z.Instance.TP_Disable == false && MobileParty.MainParty.CurrentSettlement == null && MobileParty.MainParty.MemberRoster.TotalHealthyCount >= 5;
		}
		public override void StartEvent()
		{
			if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}
			var loot = MBRandom.RandomInt(minloot, maxloot);
			var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();
			var settlements = Settlement.FindAll(s => s.IsTown || s.IsCastle || s.IsVillage).ToList();
			var ClosestSettlement = settlements.MinBy(s => MobileParty.MainParty.GetPosition().DistanceSquared(s.GetPosition()));
			string cultureclass = ClosestSettlement.Culture.ToString();

			var eventTitle = new TextObject("{=TravellingMerchant_Title}Travelling Merchant").ToString();
			
			var eventDescription = new TextObject("{=TravellingMerchant_Event_Desc}Not too far from {closestSettlement} you are met by what appears to be a lone caravan master.  He looks a bit down on his luck, as if he had recently been in quite a fight." +
				"  To your surpise he approaches your party, asking if you're interested in buying some of his wares.")
				.SetTextVariable("closestSettlement", closestSettlement)
				.ToString();
			
			var eventOutcome1 = new TextObject("{=TravellingMerchant_Event_Text_1}You ask to see the merchant's wares, he proceeds to open his cart and it appears empty except a few bits of food scrap." +
				" After a bit of friendly conversation he confesses he had been robbed by some looters not too far from here, the only thing they didn't " +
				"take was a coinpurse strapped to his leg.  You offer the man directions to the nearest town but he seems to know where he's heading.")
				.ToString();

			var eventOutcome1c = new TextObject("{=TravellingMerchant_Event_Text_1}You demand the merchant hand over his coinpurse.  Shocked and surprised, he assumes you're only kidding.  His laughter fades quickly " +
				"once he realizes you're being serious.  A few of your men take a step forward in an attempt to coerce the demands, an act that proves itself unnecessary as the merchant complies and pulls the large purse " +
				"out from beneath his pants.")
				.ToString();

			var eventOutcome2 = new TextObject("{=TravellingMerchant_Event_Text_2}You look around at your men, smirking as you raise one hand into the air and give the signal.  The merchant " +
				"sees what is coming and tries to bolt towards his pack mules but not before a few of your men have him on the ground.  After searching through the goods you realize there isn't " +
				"anything of value here other than some bits of food scrap.  You ask the merchant why his cart is empty and he confesses he was robbed not too far from here.  After looking through every " +
				"inch of the cart your men convince you there's nothing to be found.")
				.ToString();

			var eventOutcome3 = new TextObject("{=TravellingMerchant_Event_Text_3}You pass by the merchant without so much as a glance.")
				.ToString();
				
			var eventButtonText1 = new TextObject("{=TravellingMerchant_Event_Button_Text_1}Choose").ToString();
			var eventButtonText2 = new TextObject("{=TravellingMerchant_Event_Button_Text_2}Done").ToString();
			
			var eventOption1 = new TextObject("{=TravellingMerchant_Event_Option_1}Barter").ToString();
			var eventOption1Hover = new TextObject("{=TravellingMerchant_Event_Option_1_Hover}No harm in looking.").ToString();

			var eventOption2 = new TextObject("{=TravellingMerchant_Event_Option_1}Rob Him").ToString();
			var eventOption2Hover = new TextObject("{=TravellingMerchant_Event_Option_1_Hover}This man must be a fool.").ToString();

			var eventOption3 = new TextObject("{=TravellingMerchant_Event_Option_2}Decline").ToString();
			var eventOption3Hover = new TextObject("{=TravellingMerchant_Event_Option_2_Hover}No time for this.").ToString();

			var inquiryElements = new List<InquiryElement>
			{
				new InquiryElement("a", eventOption1, null, true, eventOption1Hover),
				new InquiryElement("b", eventOption2, null, true, eventOption2Hover),
				new InquiryElement("c", eventOption3, null, true, eventOption3Hover)
			};

			var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, eventButtonText1, null,
				elements => 
				{
					switch ((string)elements[0].Identifier)
					{
						case "a": //Barter -------------------------------------------------------==============================================================
							{
								Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 25);
								var eventOption1a = new TextObject("{=TravellingMerchant_Event_Option_1}Let him go").ToString();
								var eventOption1Hovera = new TextObject("{=TravellingMerchant_Event_Option_1_Hover}He's been through enough.").ToString();
								var eventOption1b = new TextObject("{=TravellingMerchant_Event_Option_1}Demand the coinpurse").ToString();
								var eventOption1Hoverb = new TextObject("{=TravellingMerchant_Event_Option_1_Hover}It's a tough life.").ToString();
								var eventOption1c = new TextObject("{=TravellingMerchant_Event_Option_1}Capture").ToString();
								var eventOption1Hoverc = new TextObject("{=TravellingMerchant_Event_Option_1_Hover}He's worth more as labor.").ToString();
								var eventOutcome1a = new TextObject("{=TravellingMerchant_Event_Text_1}You leave the merchant in peace, Lord knows he has been through enough today.")
									.ToString();
								var eventOutcome2a = new TextObject("{=TravellingMerchant_Event_Text_2}You demand the merchant hand over his coinpurse, with defeat in his eyes he pulls out the purse and hands it over.")
									.ToString();
								var eventOutcome3a = new TextObject("{=TravellingMerchant_Event_Text_3}You order your men to subdue the merchant.  His cries for help fall on deaf ears. Unlike the looters, you don't forget the coinpurse.")
									.ToString();
								var inquiryElements1 = new List<InquiryElement>
											{
											new InquiryElement("1a", eventOption1a, null, true, eventOption1Hovera),
											new InquiryElement("1b", eventOption1b, null, true, eventOption1Hoverb),
											new InquiryElement("1c", eventOption1c, null, true, eventOption1Hoverc)
											};
								var msid1 = new MultiSelectionInquiryData(eventTitle, eventOutcome1, inquiryElements1, false, 1, eventButtonText1, null,
								elements1 =>
								{
								switch ((string)elements1[0].Identifier)
									{
										case "1a"://Let him go-----------
											InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome1a, true, false, eventButtonText2, null, null, null), true);
											break;
										case "1b"://Demand coin purse ---
											Hero.MainHero.ChangeHeroGold(+loot);

											var eventOption2a = new TextObject("{=TravellingMerchant_Event_Option_1}Let him go").ToString();
											var eventOption2Hovera = new TextObject("{=TravellingMerchant_Event_Option_1_Hover}He's been through enough.").ToString();
											var eventOption2b = new TextObject("{=TravellingMerchant_Event_Option_1}Capture").ToString();
											var eventOption2Hoverb = new TextObject("{=TravellingMerchant_Event_Option_1_Hover}He's worth more as labor.").ToString();

											var eventOutcome2d = new TextObject("{=TravellingMerchant_Event_Text_3}You order your men to subdue the merchant.  His cries for help fall on deaf ears.")
											.ToString();

											Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 150);
											string eventMsg1 = new TextObject(
											"{=Refugees_Event_Msg_1}You robbed merchant for {Loot} gold")
											.SetTextVariable("Loot", loot)
											.ToString();
											InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_POS_Outcome));

											var inquiryElements2a = new List<InquiryElement>
											{
											new InquiryElement("2a", eventOption2a, null, true, eventOption2Hovera),
											new InquiryElement("2b", eventOption2b, null, true, eventOption2Hoverb)
											};

											var msid2 = new MultiSelectionInquiryData(eventTitle, eventOutcome1c, inquiryElements2a, false, 1, eventButtonText1, null,
											elements2a =>
											  {
												  switch ((string)elements2a[0].Identifier)
												  {
													  case "2a": // Let Him Go--------
														  InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome1a, true, false, eventButtonText2, null, null, null), true);
														  break;
													  case "2b": // Capture --------
														  InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome2d, true, false, eventButtonText2, null, null, null), true);
														  Hero.MainHero.ChangeHeroGold(+loot);
														  Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 300);
														  TroopRoster troopRoster2 = TroopRoster.CreateDummyTroopRoster();
														  for (int i = 0; i < CharacterObject.All.Count; i++)
														  {
															  CharacterObject characterObject = CharacterObject.All[i];

															  if (characterObject.StringId.Contains("caravan_master") && !characterObject.StringId.Contains("conspiracy") && characterObject.Culture.ToString() == cultureclass)
															  {
																  troopRoster2.AddToCounts(characterObject, 1);
															  }
														  }
														  TroopRoster emptyTroopRoster = TroopRoster.CreateDummyTroopRoster();
														  TaleWorlds.CampaignSystem.Party.PartyScreenManager.OpenScreenAsLoot(emptyTroopRoster, troopRoster2, new TextObject("{cultureclass} Merchant").SetTextVariable("cultureclass", cultureclass), 20, null);
														  string eventMsg2 = new TextObject(
														  "{=Refugees_Event_Msg_1}You subdued the merchant")
														  .SetTextVariable("Loot", loot)
														  .ToString();
														  InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color_POS_Outcome));
														  break;
												  }

											  },
																null);
																MBInformationManager.ShowMultiSelectionInquiry(msid2, true);
																StopEvent();


											break;
										case "1c"://Capture -------------
											InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome3a, true, false, eventButtonText2, null, null, null), true);
											Hero.MainHero.ChangeHeroGold(+loot);
											Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 300);
											TroopRoster troopRoster2 = TroopRoster.CreateDummyTroopRoster();
											for (int i = 0; i < CharacterObject.All.Count; i++)
											{
												CharacterObject characterObject = CharacterObject.All[i];

												if (characterObject.StringId.Contains("caravan_master") && !characterObject.StringId.Contains("conspiracy") && characterObject.Culture.ToString() == cultureclass)
												{
													troopRoster2.AddToCounts(characterObject, 1);
												}
											}
											TroopRoster emptyTroopRoster = TroopRoster.CreateDummyTroopRoster();
											TaleWorlds.CampaignSystem.Party.PartyScreenManager.OpenScreenAsLoot(emptyTroopRoster, troopRoster2, new TextObject("{cultureclass} Merchant").SetTextVariable("cultureclass", cultureclass), 20, null);
											string eventMsg2 = new TextObject(
											"{=Refugees_Event_Msg_1}You subdued the merchant")
											.SetTextVariable("Loot", loot)
											.ToString();
											InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color_POS_Outcome));
											break;																										
									}
								},
								null);
								MBInformationManager.ShowMultiSelectionInquiry(msid1, true);
								StopEvent();
							}
							break;
						case "b"://Rob Him -------------------------------------------------------
					{ 
							var eventOption2a = new TextObject("{=TravellingMerchant_Event_Option_1}Let him go").ToString();
							var eventOption2Hovera = new TextObject("{=TravellingMerchant_Event_Option_1_Hover}He's been through enough.").ToString();
							var eventOption2b = new TextObject("{=TravellingMerchant_Event_Option_1}Capture").ToString();
							var eventOption2Hoverb = new TextObject("{=TravellingMerchant_Event_Option_1_Hover}He's worth more as labor.").ToString();
							var eventOutcome2a = new TextObject("{=TravellingMerchant_Event_Text_1}You leave the merchant in peace, Lord knows he has been through enough today.")
							.ToString();
							var eventOutcome2c = new TextObject("{=TravellingMerchant_Event_Text_3}You order your men to subdue the merchant.  His cries for help fall on deaf ears.")
							.ToString();
							var inquiryElements2 = new List<InquiryElement>
											{
											new InquiryElement("1a", eventOption2a, null, true, eventOption2Hovera),
											new InquiryElement("1b", eventOption2b, null, true, eventOption2Hoverb)
											};
								Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 150);

								var msid2 = new MultiSelectionInquiryData(eventTitle, eventOutcome2, inquiryElements2, false, 1, eventButtonText1, null,
							elements2 =>
							{
								switch ((string)elements2[0].Identifier)
								{
								case "1a"://Let him go-----------
											InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome2a, true, false, eventButtonText2, null, null, null), true);
									break;
								case "1b"://Capture -------------
											InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome2c, true, false, eventButtonText2, null, null, null), true);
									Hero.MainHero.ChangeHeroGold(+loot);
										Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 300);
										TroopRoster troopRoster2 = TroopRoster.CreateDummyTroopRoster();
									for (int i = 0; i < CharacterObject.All.Count; i++)
									{
										CharacterObject characterObject = CharacterObject.All[i];
										if (characterObject.StringId.Contains("caravan_master") && !characterObject.StringId.Contains("conspiracy") && characterObject.Culture.ToString() == cultureclass)
										{
											troopRoster2.AddToCounts(characterObject, 1);
										}
									}
									TroopRoster emptyTroopRoster = TroopRoster.CreateDummyTroopRoster();
									TaleWorlds.CampaignSystem.Party.PartyScreenManager.OpenScreenAsLoot(emptyTroopRoster, troopRoster2, new TextObject("{cultureclass} Merchant").SetTextVariable("cultureclass", cultureclass), 20, null);
										string eventMsg3 = new TextObject(
										"{=Refugees_Event_Msg_1}You subdued the merchant")
										.SetTextVariable("Loot", loot)
										.ToString();
										InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color_POS_Outcome));
										break;
								}
							},
						null);
						MBInformationManager.ShowMultiSelectionInquiry(msid2, true);
						StopEvent();
					}
				break;
						case "c"://Ignore --------------------------------------------------------
							InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome3, true, false, eventButtonText2, null, null, null), true);
							break;
						default:
							MessageBox.Show($"Error while selecting option for \"{randomEventData.eventType}\"");
							break;
					}
				},
				null);

			MBInformationManager.ShowMultiSelectionInquiry(msid, true);

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


	public class TravellingmerchantData : RandomEventData
	{
		public readonly int minloot;
		public readonly int maxloot;

		public TravellingmerchantData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{

		}

		public override BaseEvent GetBaseEvent()
		{
			return new TravellingMerchant();
		}
	}
}
