﻿using System;
using System.Globalization;
using System.Windows.Data;
using TCC.Data;
using TCC.ViewModels;

namespace TCC.Converters
{
    public class ChatChannelToName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ch = (ChatChannel)value;
            switch (ch)
            {
                case ChatChannel.PartyNotice:
                    return "Notice";
                case ChatChannel.RaidNotice:
                    return "Notice";
                case ChatChannel.GuildAdvertising:
                    return "G. Ad";
                case ChatChannel.Megaphone:
                    return "Megaphone";
                case ChatChannel.Private1:
                    return ChatWindowManager.Instance.PrivateChannels[0].Name == null ? ch.ToString() : ChatWindowManager.Instance.PrivateChannels[0].Name;
                case ChatChannel.Private2:
                    return ChatWindowManager.Instance.PrivateChannels[1].Name == null ? ch.ToString() : ChatWindowManager.Instance.PrivateChannels[1].Name;
                case ChatChannel.Private3:
                    return ChatWindowManager.Instance.PrivateChannels[2].Name == null ? ch.ToString() : ChatWindowManager.Instance.PrivateChannels[2].Name;
                case ChatChannel.Private4:
                    return ChatWindowManager.Instance.PrivateChannels[3].Name == null ? ch.ToString() : ChatWindowManager.Instance.PrivateChannels[3].Name;
                case ChatChannel.Private5:
                    return ChatWindowManager.Instance.PrivateChannels[4].Name == null ? ch.ToString() : ChatWindowManager.Instance.PrivateChannels[4].Name;
                case ChatChannel.Private6:
                    return ChatWindowManager.Instance.PrivateChannels[5].Name == null ? ch.ToString() : ChatWindowManager.Instance.PrivateChannels[5].Name;
                case ChatChannel.Private7:
                    return ChatWindowManager.Instance.PrivateChannels[6].Name == null ? ch.ToString() : ChatWindowManager.Instance.PrivateChannels[6].Name;
                case ChatChannel.Private8:
                    return ChatWindowManager.Instance.PrivateChannels[7].Name == null ? ch.ToString() : ChatWindowManager.Instance.PrivateChannels[7].Name;
                case ChatChannel.Party:
                    return ch.ToString();
                case ChatChannel.Guild:
                    return ch.ToString();
                case ChatChannel.Area:
                    return ch.ToString();
                case ChatChannel.Trade:
                    return ch.ToString();
                case ChatChannel.Greet:
                    return ch.ToString();
                case ChatChannel.Emote:
                    return ch.ToString();
                case ChatChannel.Global:
                    return ch.ToString();
                case ChatChannel.Raid:
                    return ch.ToString();
                case ChatChannel.System:
                    return ch.ToString();
                case ChatChannel.Notify:
                    return "Info";
                case ChatChannel.Event:
                    return ch.ToString();
                case ChatChannel.Error:
                    return "Alert";
                case ChatChannel.Group:
                    return ch.ToString();
                case ChatChannel.GuildNotice:
                    return "Guild";
                case ChatChannel.Deathmatch:
                    return ch.ToString();
                case ChatChannel.ContractAlert:
                    return ch.ToString();
                case ChatChannel.GroupAlerts:
                    return "Group";
                case ChatChannel.Loot:
                    return ch.ToString();
                case ChatChannel.Exp:
                    return ch.ToString();
                case ChatChannel.Money:
                    return ch.ToString();
                case ChatChannel.SentWhisper:
                    return ch.ToString();
                case ChatChannel.ReceivedWhisper:
                    return ch.ToString();
                case ChatChannel.TradeRedirect:
                    return "Global";
                case ChatChannel.Enchant12:
                    return "+12";
                case ChatChannel.Enchant15:
                    return "+15";
                case ChatChannel.Enchant7:
                    return "+7";
                case ChatChannel.Enchant8:
                    return "+8";
                case ChatChannel.Enchant9:
                    return "+9";
                case ChatChannel.RaidLeader:
                    return "Leader";
                case ChatChannel.Bargain:
                    return "Offer";
                case ChatChannel.WorldBoss:
                    return "WB";
                case ChatChannel.SystemDefault:
                    return "System";
                default:
                    return ch.ToString();
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}