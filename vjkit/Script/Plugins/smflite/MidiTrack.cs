/* 
 * Copyright (C) 2013 Keijiro Takahashi
 * 
 * Permission is hereby granted, free of charge, to any 
 * person obtaining a copy of this software and associated 
 * documentation files (the "Software"), to deal in the 
 * Software without restriction, including without limitation 
 * the rights to use, copy, modify, merge, publish, distribute, 
 * sublicense, and/or sell copies of the Software, and to permit 
 * persons to whom the Software is furnished to do so, subject 
 * to the following conditions: The above copyright notice and 
 * this permission notice shall be included in all copies or 
 * substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
 * OTHER DEALINGS IN THE SOFTWARE.
 * 
 * Special Thanks:
 * The original software design and architecture of fuZe vjkit 
 * is inspired by Visualizer Studio, created by Altered Reality 
 * Entertainment LLC.
 */
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