﻿using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using TCC.Data;

namespace TCC.Controls
{
    /// <summary>
    /// Logica di interazione per EdgeControl.xaml
    /// </summary>
    public partial class EdgeControl
    {
        public EdgeControl()
        {
            InitializeComponent();
            //baseBorder.Background = (SolidColorBrush)App.Current.FindResource("DefaultBackgroundColor");

        }
        private int _currentEdge;

        private void SetEdge(int newEdge)
        {
            var diff = newEdge - _currentEdge;

            if (diff == 0) return;
            if (diff > 0)
            {
                for (var i = 0; i < diff; i++)
                {
                    if (_currentEdge + i < EdgeContainer.Children.Count - 1) EdgeContainer.Children[_currentEdge + i].Opacity = 1;
                }
            }
            else
            {
                //baseBorder.Background = (SolidColorBrush)App.Current.FindResource("DefaultBackgroundColor");
                MaxBorder.Opacity = 0;

                for (var i = EdgeContainer.Children.Count - 1; i >= 0; i--)
                {
                    EdgeContainer.Children[i].Opacity = 0;
                }
            }
            _currentEdge = newEdge;
        }

        private Counter _context;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this)) return;
            //lazy way of making sure that DataContext is not null
            while (_context == null)
            {
                _context = (Counter)DataContext;
                Thread.Sleep(500);
            }
            _context.PropertyChanged += _context_PropertyChanged;
        }

        private void _context_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Val":
                    SetEdge(_context.Val);
                    break;
                case "Maxed":
                    MaxBorder.Opacity = 1;
                    break;
            }
        }

    }
}
