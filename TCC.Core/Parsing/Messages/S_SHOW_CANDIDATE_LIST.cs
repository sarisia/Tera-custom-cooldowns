﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCC.Data;
using TCC.TeraCommon.Game.Messages;
using TCC.TeraCommon.Game.Services;

namespace TCC.Parsing.Messages
{
    public class S_SHOW_CANDIDATE_LIST : ParsedMessage
    {

        public List<User> Candidates { get; set; }

        public S_SHOW_CANDIDATE_LIST(TeraMessageReader reader) : base(reader)
        {
            var count = reader.ReadUInt16();
            var offset = reader.ReadUInt16();

            Candidates = new List<User>();
            if (count == 0) return;
            reader.BaseStream.Position = offset - 4;
            for (int i = 0; i < count; i++)
            {
                var current = reader.ReadUInt16();
                var next = reader.ReadUInt16();

                var nameOffset = reader.ReadUInt16();

                var playerId = reader.ReadUInt32();
                var cls = (Class)reader.ReadUInt16();
                reader.Skip(2 + 2);
                var level = reader.ReadUInt16();
                var worldId = reader.ReadUInt32();
                var guardId = reader.ReadUInt32();
                var sectionId = reader.ReadUInt32();

                reader.BaseStream.Position = nameOffset - 4;

                var name = reader.ReadTeraString();

                Candidates.Add(new User(WindowManager.LfgListWindow.Dispatcher)
                {
                    PlayerId = playerId,
                    UserClass = cls,
                    Level = level,
                    Location = SessionManager.MapDatabase.GetName(guardId, sectionId),
                    Online = true,
                    Name = name

                });
                if (next != 0) reader.BaseStream.Position = next - 4;
            }
        }
    }
}
