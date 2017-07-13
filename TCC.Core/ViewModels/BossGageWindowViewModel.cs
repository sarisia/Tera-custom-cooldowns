﻿using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using TCC.Data;

namespace TCC.ViewModels
{
    public class BossGageWindowViewModel : TSPropertyChanged
    {
        private static BossGageWindowViewModel _instance;
        public static BossGageWindowViewModel Instance => _instance ?? (_instance = new BossGageWindowViewModel());
        public bool HarrowholdMode
        {
            get => SessionManager.HarrowholdMode;
        }
        private HarrowholdPhase _currentHHphase;
        public HarrowholdPhase CurrentHHphase
        {
            get => _currentHHphase;
            set
            {
                if (_currentHHphase == value) return;
                _currentHHphase = value;
                NotifyPropertyChanged(nameof(CurrentHHphase));
            }
        }
        private void SessionManager_HhModeChanged(bool val)
        {
            NotifyPropertyChanged("CurrentNPCs");
            NotifyPropertyChanged("HarrowholdMode");
        }
        public bool IsTeraOnTop
        {
            get => WindowManager.IsTccVisible;
        }

        private double scale = SettingsManager.BossGaugeWindowSettings.Scale;
        public double Scale
        {
            get { return scale; }
            set
            {
                if (scale == value) return;
                scale = value;
                NotifyPropertyChanged("Scale");
            }
        }

        private SynchronizedObservableCollection<Boss> _bosses;
        public SynchronizedObservableCollection<Boss> CurrentNPCs
        {
            get
            {
                if (SessionManager.HarrowholdMode)
                {
                    return _bosses;
                }
                else
                {
                    return _bosses;
                }
            }
            set
            {
                if (_bosses == value) return;
                _bosses = value;
            }
        }
        public int VisibleBossesCount
        {
            get => CurrentNPCs.Where(x => x.Visible == Visibility.Visible).Count();
        }
        public void AddOrUpdateBoss(ulong entityId, float maxHp, float curHp, uint templateId = 0, uint zoneId = 0, Visibility v = Visibility.Visible)
        {

            var boss = _bosses.FirstOrDefault(x => x.EntityId == entityId);
            if (boss == null)
            {
                //if (!EntitiesManager.TryGetBossById(entityId, out Boss b)) return;
                if (templateId == 0 || zoneId == 0) return;
                boss = new Boss(entityId, zoneId, templateId, v);
                _bosses.Add(boss);
            }
            boss.MaxHP = maxHp;
            boss.CurrentHP = curHp;
            boss.Visible = v;         
        }
        public void RemoveBoss(ulong id)
        {
            var boss = _bosses.FirstOrDefault(x => x.EntityId == id);
            if (boss == null) return;
            _bosses.Remove(boss);
            boss.Dispose();
        }
        internal void CopyToClipboard()
        {
            var sb = new StringBuilder();
            foreach (var boss in _bosses)
            {
                if(boss.Visible == Visibility.Visible)
                {
                    sb.Append(boss.Name);
                    sb.Append(": ");
                    sb.Append(String.Format("{0:##0%}",boss.CurrentPercentage));
                    sb.Append("\\");
                }
            }
            Clipboard.SetText(sb.ToString());
        }
        public void ClearBosses()
        {
            _dispatcher.Invoke(() =>
            {
                _bosses.Clear();
            });
        }
        public BossGageWindowViewModel()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            SessionManager.HhModeChanged += SessionManager_HhModeChanged;
            WindowManager.TccVisibilityChanged += (s, ev) =>
            {
                NotifyPropertyChanged("IsTeraOnTop");
                if (IsTeraOnTop)
                {
                    _dispatcher.Invoke(() =>
                    {
                        WindowManager.BossGauge.Topmost = false;
                        WindowManager.BossGauge.Topmost = true;
                    });
                }
            };

        }
        internal void EndNpcAbnormality(ulong target, Abnormality ab)
        {
            var boss = _bosses.FirstOrDefault(x => x.EntityId == target);
            if(boss != null)
            {
                boss.EndBuff(ab);
            }
        }
        internal void AddOrRefreshNpcAbnormality(Abnormality ab, int stacks, uint duration, ulong target, double size, double margin)
        {
            var boss = _bosses.FirstOrDefault(x => x.EntityId == target);
            if (boss != null)
            {
                boss.AddorRefresh(ab, duration, stacks, size, margin);
            }
        }
        internal void SetBossEnrage(ulong entityId, bool enraged)
        {
            var boss = _bosses.FirstOrDefault(x => x.EntityId == entityId);
            if (boss == null)
            {
                return;
            }
            boss.Enraged = enraged;
        }
        internal void UnsetBossTarget(ulong entityId)
        {
            var boss = _bosses.FirstOrDefault(x => x.EntityId == entityId);
            if (boss == null)
            {
                return;
            }
            boss.Target = 0;
        }
        internal void SetBossAggro(ulong entityId, AggroCircle circle, ulong user)
        {
            var boss = _bosses.FirstOrDefault(x => x.EntityId == entityId);
            if (boss == null)
            {
                return;
            }
            boss.Target = user;
            boss.CurrentAggroType = AggroCircle.Main;
        }
    }
}
