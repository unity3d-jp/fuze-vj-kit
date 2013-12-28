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

        public List<MidiEvent> Start (float startTime = 0.0f)
        {
            if (enumerator.MoveNext ()) {
                pulseToNext = enumerator.Current.delta;
                playing = true;
				return Advance (startTime);
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