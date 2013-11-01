using System.Collections.Generic;

namespace SmfLite
{
    public struct MidiFileContainer
    {
        public int division;
        public List<MidiTrack> tracks;

        public MidiFileContainer (int division, List<MidiTrack> tracks)
        {
            this.division = division;
            this.tracks = tracks;
        }

        public override string ToString ()
        {
            var temp = division.ToString () + ",";
            foreach (var track in tracks) {
                temp += track;
            }
            return temp;
        }
    }
}