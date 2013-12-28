//
// MidiOut.cs - MIDI output manager
//
// Copyright (C) 2013 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;

public static class MidiOut
{
    public static void SendNoteOn(MidiChannel channel, int noteNumber, float velocity)
    {
        int cn = Mathf.Clamp ((int)channel, 0, 15);
        noteNumber = Mathf.Clamp (noteNumber, 0, 127);
        velocity = Mathf.Clamp (127.0f * velocity, 0.0f, 127.0f);
        MidiBridge.instance.Send (0x90 + cn, noteNumber, (int)velocity);
    }

    public static void SendNoteOff(MidiChannel channel, int noteNumber)
    {
        int cn = Mathf.Clamp ((int)channel, 0, 15);
        noteNumber = Mathf.Clamp (noteNumber, 0, 127);
        MidiBridge.instance.Send (0x80 + cn, noteNumber, 0);
    }

    public static void SendControlChange(MidiChannel channel, int controlNumber, float value)
    {
        int cn = Mathf.Clamp ((int)channel, 0, 15);
        controlNumber = Mathf.Clamp (controlNumber, 0, 127);
        value = Mathf.Clamp (127.0f * value, 0.0f, 127.0f);
        MidiBridge.instance.Send (0xb0 + cn, controlNumber, (int)value);
    }
}
