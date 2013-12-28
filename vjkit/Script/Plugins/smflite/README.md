smflite
=======

**smflite** is a minimal toolkit to handle standard MIDI files (SMF) on Unity.

What it is used for
-------------------

- Audio visual app: synchronize animations to pre-recorded audio clips.
- Rhythm games: retrieve timing information from song data.

What it can't be used for
-------------------------

- Playback MIDI songs.
- Retrieve meta-data or SysEx data from SMF.

How to use
----------

First of all, load a MIDI file with `MidiFileLoader.Load`. It returns
a `MidiFileContainer` instance which contains the song data.

```C#
MidiFileContainer song = MidiFileLoader.Load (smfAsset.bytes);
```

And then create `MidiTrackSequencer` with the song data.
You can specify the BPM for playback here.

```C#
seq = new MidiTrackSequencer (song.tracks[0], song.division, bpm);
```

Call the `Play` method in the sequencer class. It return a set of
`MidiEvent` on the starting point of the song.

```C#
foreach (MidiEvent e in seq.Start ()) {
  // Do something with a MidiEvent.
}
```

Call the `Advance` method in every frame. You should give a delta-time,
and then it returns a set of `MidiEvent` which occurred between
the previous frame and the current frame.

```C#
void Update() {
  if (seq.Playing) {
    foreach (MidiEvent e in seq.Advance (Time.deltaTime)) {
      // Do something with a MidiEvent.
    }
  }
}
```

You can run it until the `Playing` property becomes false.

For the detailed usage, see an example
[here](https://github.com/keijiro/unity-smf-test).

License
-------

Copyright (C) 2013 Keijiro Takahashi

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.



