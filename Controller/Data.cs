﻿using Model;
using System;

namespace Controller
{
    public static class Data
    {
        public static Competition Competition { get; private set; }
        public static Race CurrentRace { get; private set; }

        public static event EventHandler<NextRaceEventArgs> NextRaceEvent;

        private static bool _lastRaceFinished;

        public static void Initialize()
        {
            Competition = new Competition();
            AddParticipants();
            AddTracks();
        }

        private static void AddParticipants()
        {
            Competition.Participants.Add(new Driver("Jaap", 0, new Car(8, 10, 16, false), TeamColors.Red));
            Competition.Participants.Add(new Driver("Sjaak", 0, new Car(12, 10, 20, false), TeamColors.Green));
            Competition.Participants.Add(new Driver("Manus", 0, new Car(20, 10, 18, false), TeamColors.Blue));
            Competition.Participants.Add(new Driver("Arie", 0, new Car(14, 10, 20, false), TeamColors.Orange));
            Competition.Participants.Add(new Driver("Lars", 0, new Car(20, 10, 24, false), TeamColors.Yellow));
            Competition.Participants.Add(new Driver("Elsa", 0, new Car(16, 10, 30, false), TeamColors.Pink));
        }

        private static void AddTracks()
        {
            Competition.Tracks.Enqueue(new Track("Rivendell", new[]
            {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish,
                SectionTypes.LeftCorner,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner
            }));

            Competition.Tracks.Enqueue(new Track("El Norte", new[]
            {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.LeftCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight
            }));

            Competition.Tracks.Enqueue(new Track("The Oval", new[]
            {
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.StartGrid,
                SectionTypes.Finish,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.Straight,
                SectionTypes.RightCorner,
                SectionTypes.Straight,
                SectionTypes.RightCorner
            }));
        }

        public static void NextRace()
        {
            // cleanup previous race
            CurrentRace?.CleanUp();

            // get next track from competitionData, then perform null check. when not null create a new race.
            Track currentTrack = Competition.NextTrack();
            if (currentTrack != null)
            {
                CurrentRace = new Race(currentTrack, Competition.Participants);
                CurrentRace.RaceFinished += OnRaceFinished;
                NextRaceEvent?.Invoke(null, new NextRaceEventArgs() { Race = CurrentRace });
                CurrentRace.Start();
            }
            else if (!_lastRaceFinished) // On the last race, call NextRaceEvent one more time to update all data contexts.
            {
                NextRaceEvent?.Invoke(null, new NextRaceEventArgs() { Race = CurrentRace });
                _lastRaceFinished = true;
            }
        }

        private static void OnRaceFinished(object sender, EventArgs e)
        {
            Competition.DeterminePoints(CurrentRace.GetFinishOrderParticipants());
            Competition.StoreRaceLength(CurrentRace.Track.Name, CurrentRace.GetRaceLength());
            Competition.StoreParticipantsSpeed(CurrentRace.GetParticipantSpeeds());
            NextRace();
        }
    }
}