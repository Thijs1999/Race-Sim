﻿using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class ParticipantSpeed : IStorageConstraint
    {
        public string Name { get; set; }
        public string TrackName { get; set; }
        public int Speed { get; set; }

        public void Add<T>(List<T> list) where T : class, IStorageConstraint
        {
            list.Add(this as T);
        }

        public string BestParticipant<T>(List<T> list) where T : class, IStorageConstraint
        {
            ParticipantSpeed highestSpeed = null;

            foreach (var currentParticipantSpeed in list.Select(storageConstraint => storageConstraint as ParticipantSpeed))
            {
                highestSpeed ??= currentParticipantSpeed;
                if (currentParticipantSpeed.Speed > highestSpeed.Speed)
                {
                    highestSpeed = currentParticipantSpeed;
                }
            }

            return highestSpeed.Name;
        }

        public override string ToString()
        {
            return $"{Name}, {Speed} on {TrackName}";
        }
    }
}