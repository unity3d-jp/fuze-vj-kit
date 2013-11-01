using System.Collections.Generic;
using DeltaEventPairList = System.Collections.Generic.List<SmfLite.MidiTrack.DeltaEventPair>;

namespace SmfLite
{
    public class MidiTrackSequencer
    {
        DeltaEventPairList.Enumerator enumerator;
        bool playing;
        float pulsePerSecond;
        float pulseToNext;
        float pulseCounter;

        public bool Playing {
            get { return playing; }
        }

        public MidiTrackSequencer (MidiTrack track, int ppqn, float bpm)
        {
            pulsePerSecond = bpm / 60.0f * ppqn;
            enumerator = track.GetEnumerator ();
        }

        public List<MidiEvent> Start ()
        {
            if (enumerator.MoveNext ()) {
                pulseToNext = enumerator.Current.delta;
                playing = true;
                return Advance (0);
            } else {
                playing = false;
                return null;
            }
        }

        public List<MidiEvent> Advance (float deltaTime)
        {
            if (!playing) {
                return null;
            }

            pulseCounter += pulsePerSecond * deltaTime;

            if (pulseCounter < pulseToNext) {
                return null;
            }

            var messages = new List<MidiEvent> ();

            while (pulseCounter >= pulseToNext) {
                var pair = enumerator.Current;
                messages.Add (pair.midiEvent);
                if (!enumerator.MoveNext ()) {
                    playing = false;
                    break;
                }

                pulseCounter -= pulseToNext;
                pulseToNext = enumerator.Current.delta;
            }

            return messages;
        }
    }
}