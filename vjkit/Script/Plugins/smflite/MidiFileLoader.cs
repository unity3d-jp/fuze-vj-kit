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
    public static class MidiFileLoader
    {
        public static MidiFileContainer Load (byte[] data)
        {
            var tracks = new List<MidiTrack> ();
            var reader = new MidiDataStreamReader (data);

            // Chunk type.
            if (new string (reader.ReadChars (4)) != "MThd") {
                throw new System.FormatException ("Can't find header chunk.");
            }

            // Chunk length.
            if (reader.ReadBEInt32 () != 6) {
                throw new System.FormatException ("Length of header chunk must be 6.");
            }

            // Format (unused).
            reader.Advance (2);

            // Number of tracks.
            var trackCount = reader.ReadBEInt16 ();

            // Delta-time divisions.
            var division = reader.ReadBEInt16 ();
            if ((division & 0x8000) != 0) {
                throw new System.FormatException ("SMPTE time code is not supported.");
            }

            // Read the tracks.
            for (var trackIndex = 0; trackIndex < trackCount; trackIndex++) {
                tracks.Add (ReadTrack (reader));
            }

            return new MidiFileContainer (division, tracks);
        }

        static MidiTrack ReadTrack (MidiDataStreamReader reader)
        {
            var track = new MidiTrack ();

            // Chunk type.
            if (new string (reader.ReadChars (4)) != "MTrk") {
                throw new System.FormatException ("Can't find track chunk.");
            }
            
            // Chunk length.
            var chunkEnd = reader.ReadBEInt32 ();
            chunkEnd += reader.Offset;

            // Read delta-time and event pairs.
            byte ev = 0;
            while (reader.Offset < chunkEnd) {
                // Delta time.
                var delta = reader.ReadMultiByteValue ();

                // Event type.
                if ((reader.PeekByte () & 0x80) != 0) {
                    ev = reader.ReadByte ();
                }

                if (ev == 0xff) {
                    // 0xff: Meta event (unused).
                    reader.Advance (1);
                    reader.Advance (reader.ReadMultiByteValue ());
                } else if (ev == 0xf0) {
                    // 0xf0: SysEx (unused).
                    while (reader.ReadByte() != 0xf7) {
                    }
                } else {
                    // MIDI event
                    byte data1 = reader.ReadByte ();
                    byte data2 = ((ev & 0xe0) == 0xc0) ? (byte)0 : reader.ReadByte ();
                    track.AddEvent (delta, new MidiEvent (ev, data1, data2));
                }
            }
               
            return track;
        }
    }
}
