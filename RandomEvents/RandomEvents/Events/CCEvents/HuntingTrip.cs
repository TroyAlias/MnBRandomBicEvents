﻿using System;
using System.Windows;
using CryingBuffalo.RandomEvents.Settings;
using CryingBuffalo.RandomEvents.Settings.MCM;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class HuntingTrip : BaseEvent
    {
        private readonly int minSoldiersToGo;
        private readonly int maxSoldiersToGo;
        private readonly int maxCatch;
        private readonly int minMoraleGain;
        private readonly int maxMoraleGain;
        private readonly int minYieldMultiplier;
        private readonly int maxYieldMultiplier;
        

        public HuntingTrip() : base(ModSettings.RandomEvents.HuntingTripData)
        {
            minSoldiersToGo = MCM_MenuConfig_A_M.Instance.HT_MinSoldiersToGo;
            maxSoldiersToGo = MCM_MenuConfig_A_M.Instance.HT_MaxSoldiersToGo;
            maxCatch = MCM_MenuConfig_A_M.Instance.HT_MaxCatch;
            minMoraleGain = MCM_MenuConfig_A_M.Instance.HT_MinMoraleGain;
            maxMoraleGain = MCM_MenuConfig_A_M.Instance.HT_MaxMoraleGain;
            minYieldMultiplier = MCM_MenuConfig_A_M.Instance.HT_MinYieldMultiplier;
            maxYieldMultiplier = MCM_MenuConfig_A_M.Instance.HT_MaxYieldMultiplier;
        }

        public override void CancelEvent()
        {
        }

        public override bool CanExecuteEvent()
        {
            
            return MCM_MenuConfig_A_M.Instance.HT_Disable == false && MobileParty.MainParty.MemberRoster.TotalRegulars >= 50;
        }

        public override void StartEvent()
        {
            if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
            }

            var eventTitle = new TextObject("{=HuntingTrip_Title}The Great Hunt").ToString();
            
            var yieldMultiplier = MBRandom.RandomInt(minYieldMultiplier, maxYieldMultiplier);

            var soldiersGoneHunting = MBRandom.RandomInt(minSoldiersToGo, maxSoldiersToGo);
            var animalsCaught = MBRandom.RandomInt(0, maxCatch);
            
            var yieldedMeatResources = animalsCaught * yieldMultiplier;

            var moraleGained = MBRandom.RandomInt(minMoraleGain, maxMoraleGain);
            
            var meat = MBObjectManager.Instance.GetObject<ItemObject>("meat");
            var hides = MBObjectManager.Instance.GetObject<ItemObject>("hides");
            
            var eventDescription = new TextObject(
                    "{=HuntingTrip_Event_Desc}While camping, {soldiersGoneHunting} of your men decide they want to go into the forest just west of camp to try hunting.\n " +
                    "You could use the additional resources and it would be a great morale booster for the party if they catch some. You tell them to be back before nightfall.")
                .SetTextVariable("soldiersGoneHunting", soldiersGoneHunting)
                .ToString();
            
            var eventOutcome1 = new TextObject(
                    "{=HuntingTrip_Outcome_1}Your men return empty handed just before nightfall. At least they had a good time together.")
                .ToString();
            
            var eventOutcome2 = new TextObject(
                    "{=HuntingTrip_Outcome_2}Your hunters return just before nightfall, having successfully caught {animalsCaught} animals, yielding {animalsCaught} hides " +
                    "and {yieldedMeatResources} pieces of meat. Better than nothing. You let the hunters finish butchering the animals.")
                .SetTextVariable("animalsCaught",animalsCaught)
                .SetTextVariable("yieldedMeatResources",yieldedMeatResources)
                .ToString();
            
            var eventOutcome3 = new TextObject(
                    "{=HuntingTrip_Outcome_3}Your hunters return just before nightfall, having successfully caught {animalsCaught} animals, yielding {animalsCaught} hides " +
                    "and {yieldedMeatResources} pieces of meat. You join the hunters in storing the meat.")
                .SetTextVariable("animalsCaught",animalsCaught)
                .SetTextVariable("yieldedMeatResources",yieldedMeatResources)
                .ToString();
            
            var eventOutcome4 = new TextObject(
                    "{=HuntingTrip_Outcome_4}Your hunters return triumphantly just before nightfall, having successfully caught {animalsCaught} animals, yielding {animalsCaught} " +
                    "hides and {yieldedMeatResources} pieces of meat. You order your men to start preparing a feast for everyone.")
                .SetTextVariable("animalsCaught",animalsCaught)
                .SetTextVariable("yieldedMeatResources",yieldedMeatResources)
                .ToString();
            
            var eventButtonText = new TextObject("{=HuntingTrip_Event_Button_Text}Continue")
                .ToString();
            
            var eventMsg1 =new TextObject(
                    "{=HuntingTrip_Event_Msg_1}Your men returned empty handed but it raised morale by {moraleGained}.")
                .SetTextVariable("moraleGained", moraleGained - 3)
                .ToString();
            
            var eventMsg2 =new TextObject(
                    "{=HuntingTrip_Event_Msg_2}The hunt yielded {animalsCaught} hides and {yieldedMeatResources} pieces of meat.\n Morale was raised by {moraleGained}.")
                .SetTextVariable("animalsCaught", animalsCaught)
                .SetTextVariable("yieldedMeatResources", yieldedMeatResources)
                .SetTextVariable("moraleGained", moraleGained - 2)
                .ToString();
            
            var eventMsg3 =new TextObject(
                    "{=HuntingTrip_Event_Msg_3}The hunt yielded {animalsCaught} hides and {yieldedMeatResources} pieces of meat.\n Morale was raised by {moraleGained}.")
                .SetTextVariable("animalsCaught", animalsCaught)
                .SetTextVariable("yieldedMeatResources", yieldedMeatResources)
                .SetTextVariable("moraleGained", moraleGained - 1)
                .ToString();
            
            var eventMsg4 =new TextObject(
                    "{=HuntingTrip_Event_Msg_4}The hunt yielded {animalsCaught} hides and {yieldedMeatResources} pieces of meat.\n Morale was raised by {moraleGained}.")
                .SetTextVariable("animalsCaught", animalsCaught)
                .SetTextVariable("yieldedMeatResources", yieldedMeatResources)
                .SetTextVariable("moraleGained", moraleGained)
                .ToString();
            

            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription, true, false, eventButtonText, null, null, null), true);
            
            MobileParty.MainParty.ItemRoster.AddToCounts(meat, yieldedMeatResources);
            MobileParty.MainParty.ItemRoster.AddToCounts(hides, animalsCaught);

            if (animalsCaught == 0)
            {
                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome1, true, false, eventButtonText, null, null, null), true);
                
                MobileParty.MainParty.RecentEventsMorale += moraleGained - 3;
                MobileParty.MainParty.MoraleExplained.Add(moraleGained);
                
                InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color));
            }
            else if (animalsCaught > 0 && animalsCaught <= 5)
            {
                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome2, true, false, eventButtonText, null, null, null), true);
                
                MobileParty.MainParty.RecentEventsMorale += moraleGained - 2;
                MobileParty.MainParty.MoraleExplained.Add(moraleGained);
                
                InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color));
            }
            else if (animalsCaught > 5 && animalsCaught <= 15)
            {
                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome3, true, false, eventButtonText, null, null, null), true);
                
                MobileParty.MainParty.RecentEventsMorale += moraleGained - 1;
                MobileParty.MainParty.MoraleExplained.Add(moraleGained);
                
                InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color));
            }
            else if (animalsCaught > 15 && animalsCaught <= maxCatch)
            {
                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome4, true, false, eventButtonText, null, null, null), true);
                
                MobileParty.MainParty.RecentEventsMorale += moraleGained;
                MobileParty.MainParty.MoraleExplained.Add(moraleGained);
                
                InformationManager.DisplayMessage(new InformationMessage(eventMsg4, RandomEventsSubmodule.Msg_Color));
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
                MessageBox.Show(
                    $"Error while stopping \"{randomEventData.eventType}\" event :\n\n {ex.Message} \n\n {ex.StackTrace}");
            }
        }
    }


    public class HuntingTripData : RandomEventData
    {

        public HuntingTripData(string eventType, float chanceWeight) : base(eventType,
            chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new HuntingTrip();
        }
    }
}