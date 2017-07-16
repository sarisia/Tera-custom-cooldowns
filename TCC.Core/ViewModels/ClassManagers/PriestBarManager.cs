﻿using System;
using System.IO;
using System.Xml.Linq;
using TCC.Data;
using TCC.Data.Databases;

namespace TCC.ViewModels
{
    public class PriestBarManager : ClassManager
    {
        private static PriestBarManager _instance;
        public static PriestBarManager Instance => _instance ?? (_instance = new PriestBarManager());

        public DurationCooldownIndicator EnergyStars { get; private set; }

        public PriestBarManager() : base()
        {
            _instance = this;
            CurrentClassManager = this;
            LoadSpecialSkills();
        }
        protected override void LoadSpecialSkills()
        {
            //Energy Stars
            EnergyStars = new DurationCooldownIndicator(_dispatcher);
            SkillsDatabase.TryGetSkill(350410, Class.Priest, out Skill es);
            EnergyStars.Cooldown = new FixedSkillCooldown(es, CooldownType.Skill, _dispatcher, true);
            EnergyStars.Buff = new FixedSkillCooldown(es, CooldownType.Skill, _dispatcher, false);
        }

        public override bool StartSpecialSkill(SkillCooldown sk)
        {
            if(sk.Skill.IconName == EnergyStars.Cooldown.Skill.IconName)
            {
                EnergyStars.Cooldown.Start(sk.Cooldown);
                return true;
            }
            return false;
        }
    }
}