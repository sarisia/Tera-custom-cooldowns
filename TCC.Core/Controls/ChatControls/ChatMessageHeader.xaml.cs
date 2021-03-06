﻿using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;
using TCC.Data;
using TCC.Parsing;

namespace TCC.Controls.ChatControls
{
    /// <summary>
    /// Logica di interazione per ChatMessageHeader.xaml
    /// </summary>
    public partial class ChatMessageHeader
    {
        public ChatMessageHeader()
        {
            InitializeComponent();
        }

        private void OutlinedTextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var context = (ChatMessage)DataContext;
            if (context.Author == "System" || context.Channel == ChatChannel.Twitch) return;
            Proxy.AskInteractive(PacketProcessor.Server.ServerId, context.Author);
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            var s = sender as ContentControl;
            if (s?.Effect is DropShadowEffect eff) eff.Opacity = .7;
        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
            var s = sender as ContentControl;
            if (s?.Effect is DropShadowEffect eff) eff.Opacity = 0;
        }
    }
}
