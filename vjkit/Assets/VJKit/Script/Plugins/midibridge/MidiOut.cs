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
using UnityEngine;

public static class MidiOut
{
    public static void SendNoteOn(int channel, int noteNumber, float velocity)
    {
        velocity = Mathf.Clamp (127.0f * velocity, 0.0f, 127.0f);
        MidiBridge.instance.Send (0x90 + channel, noteNumber, (int)velocity);
    }

    public static void SendNoteOff(int channel, int noteNumber)
    {
        MidiBridge.instance.Send (0x80 + channel, noteNumber, 0);
    }

    public static void SendControlChange(int channel, int controlChannel, float value)
    {
        value = Mathf.Clamp (127.0f * value, 0.0f, 127.0f);
        MidiBridge.instance.Send (0xb0 + channel, controlChannel, (int)value);
    }
}
