﻿using System;

namespace Model
{
    public class SectionData
    {
        public IParticipant Left { get; set; }
        public int DistanceLeft { get; set; }
        public IParticipant Right { get; set; }
        public int DistanceRight { get; set; }

        public DateTime StartTimeLeft { get; set; }

        public DateTime StartTimeRight { get; set; }
    }
}