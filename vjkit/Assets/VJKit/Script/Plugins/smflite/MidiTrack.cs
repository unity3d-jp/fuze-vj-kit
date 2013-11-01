using System.Collections.Generic;

namespace SmfLite
{
    // MIDI track class.
    public class MidiTrack
    {
        public struct DeltaEventPair
        {
            public int delta;
            public MidiEvent midiEvent;

            public DeltaEventPair (int delta, MidiEvent midiEvent)
            {
                this.delta = delta;
                this.midiEvent = midiEvent;
            }

            public override string ToString ()
            {
                return "(" + delta.ToString ("X") + ":" + midiEvent + ")";
            }
        }

        List<DeltaEventPair> sequence;

        public MidiTrack ()
        {
            sequence = new List<DeltaEventPair> ();
        }

        public void AddEvent (int delta, MidiEvent midiEvent)
        {
            sequence.Add (new DeltaEventPair (delta, midiEvent));
        }

        public List<DeltaEventPair>.Enumerator GetEnumerator ()
        {
            return sequence.GetEnumerator ();
        }

        public DeltaEventPair GetAtIndex (int index)
        {
            return sequence [index];
        }

        public override string ToString ()
        {
            var s = "";
            foreach (var pair in sequence)
                s += pair;
            return s;
        }
    }
}