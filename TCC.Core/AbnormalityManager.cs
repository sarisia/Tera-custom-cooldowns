﻿using TCC.Data;
using TCC.Data.Databases;
using TCC.ViewModels;

namespace TCC
{
    public delegate void UpdateAbnormalityEventHandler(ulong target, Abnormality ab, int duration, int stacks);

    public static class AbnormalityManager
    {
        public const double PlayerAbSize = 32;
        public const double PartyAbSize = 28;
        public const double RaidAbSize = 24;
        public const double BossAbSize = 30;
        public const double PlayerAbLeftMargin = 2;
        public const double PartyAbLeftMargin = 1;
        public const double RaidAbLeftMargin = -9;
        public const double BossAbLeftMargin = 2;


        public static AbnormalityDatabase CurrentDb;

        public static void BeginAbnormality(uint id, ulong target, uint duration, int stacks)
        {
            if (CurrentDb.Abnormalities.TryGetValue(id, out Abnormality ab))
            {
                if (!Filter(ab)) return;
                if (target == SessionManager.CurrentPlayer.EntityId)
                {
                    BeginPlayerAbnormality(ab, stacks, duration);
                    if (SettingsManager.DisablePartyAbnormals) return;
                    GroupWindowViewModel.Instance.BeginOrRefreshUserAbnormality(ab, stacks, duration, SessionManager.CurrentPlayer.PlayerId, SessionManager.CurrentPlayer.ServerId);
                }
                else
                {
                    BeginNpcAbnormality(ab, stacks, duration, target);
                }
            }
        }
        public static void EndAbnormality(ulong target, uint id)
        {
            if (CurrentDb.Abnormalities.TryGetValue(id, out Abnormality ab))
            {
                if (target == SessionManager.CurrentPlayer.EntityId)
                {
                    EndPlayerAbnormality(ab);
                    GroupWindowViewModel.Instance.EndUserAbnormality(ab, SessionManager.CurrentPlayer.PlayerId, SessionManager.CurrentPlayer.ServerId);

                }
                //else if (EntitiesManager.TryGetBossById(target, out Boss b))
                //{
                //    b.EndBuff(ab);
                //}
                else
                {
                    BossGageWindowViewModel.Instance.EndNpcAbnormality(target, ab);
                }
            }

        }
        static void BeginPlayerAbnormality(Abnormality ab, int stacks, uint duration)
        {
            if (ab.Type == AbnormalityType.Buff)
            {
                if (ab.Infinity)
                {
                    BuffBarWindowViewModel.Instance.Player.AddOrRefreshInfBuff(ab, duration, stacks, PlayerAbSize, PlayerAbLeftMargin);

                }
                else
                {
                    BuffBarWindowViewModel.Instance.Player.AddOrRefreshBuff(ab, duration, stacks, PlayerAbSize, PlayerAbLeftMargin);
                    if(ab.IsShield) SessionManager.SetPlayerMaxShield(ab.ShieldSize);
                }
            }
            else
            {
                BuffBarWindowViewModel.Instance.Player.AddOrRefreshDebuff(ab, duration, stacks, PlayerAbSize, PlayerAbLeftMargin);
                CharacterWindowViewModel.Instance.Player.AddToDebuffList(ab);
                ClassManager.SetStatus(ab, true);
            }
            CheckDragonProc(ab);
            //var sysMsg = new ChatMessage("@661\vAbnormalName\v" + ab.Name, SystemMessages.Messages["SMT_BATTLE_BUFF_DEBUFF"]);
            //ChatWindowViewModel.Instance.AddChatMessage(sysMsg);

        }

        private static void CheckDragonProc(Abnormality ab)
        {
            if (ab.Id >= 6001 && ab.Id <= 6002)
            {
                SkillManager.AddItemSkill(ab.Id, 60);
            }
        }

        static void EndPlayerAbnormality(Abnormality ab)
        {
            if (ab.Type == AbnormalityType.Buff)
            {
                if (ab.Infinity)
                {
                    BuffBarWindowViewModel.Instance.Player.RemoveInfBuff(ab);
                }
                else
                {

                    BuffBarWindowViewModel.Instance.Player.RemoveBuff(ab);
                    if (ab.IsShield) SessionManager.SetPlayerShield(0);
                    if (ab.IsShield) SessionManager.SetPlayerMaxShield(0);

                }
            }
            else
            {
                BuffBarWindowViewModel.Instance.Player.RemoveDebuff(ab);
                CharacterWindowViewModel.Instance.Player.RemoveFromDebuffList(ab);
                ClassManager.SetStatus(ab, false);
            }
        }

        static void BeginNpcAbnormality(Abnormality ab, int stacks, uint duration, ulong target)
        {
            //if (EntitiesViewModel.TryGetBossById(target, out Boss b))
            //{
            //    b.AddorRefresh(ab, duration, stacks, BOSS_AB_SIZE, BOSS_AB_LEFT_MARGIN);
            //}
            BossGageWindowViewModel.Instance.AddOrRefreshNpcAbnormality(ab, stacks, duration, target, BossAbSize, BossAbLeftMargin);
        }
        static bool Filter(Abnormality ab)
        {
            if (ab.Name.Contains("BTS") || ab.ToolTip.Contains("BTS") || !ab.IsShow) return false;
            if (ab.Name.Contains("(Hidden)") || ab.Name.Equals("Unknown") || ab.Name.Equals(string.Empty)) return false;
            return true;
        }

        public static void BeginOrRefreshPartyMemberAbnormality(uint playerId, uint serverId, uint id, uint duration, int stacks)
        {
            if (CurrentDb.Abnormalities.TryGetValue(id, out Abnormality ab))
            {
                if (!Filter(ab)) return;
                GroupWindowViewModel.Instance.BeginOrRefreshUserAbnormality(ab, stacks, duration, playerId, serverId);
            }
        }

        internal static void EndPartyMemberAbnormality(uint playerId, uint serverId, uint id)
        {
            if (CurrentDb.Abnormalities.TryGetValue(id, out Abnormality ab))
            {
                if (!Filter(ab)) return;
                GroupWindowViewModel.Instance.EndUserAbnormality(ab, playerId, serverId);
            }
        }
    }
}
