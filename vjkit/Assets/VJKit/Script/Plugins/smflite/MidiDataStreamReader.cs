namespace SmfLite
{
    // Simple binary data stream reader class.
    public class MidiDataStreamReader
    {
        byte[] data;
        int offset;

        public int Offset {
            get { return offset; }
        }

        public MidiDataStreamReader (byte[] data)
        {
            this.data = data;
        }

        public void Advance (int length)
        {
            offset += length;
        }

        public byte PeekByte ()
        {
            return data [offset];
        }

        public byte ReadByte ()
        {
            return data [offset++];
        }

        public char[] ReadChars (int length)
        {
            var temp = new char[length];
            for (var i = 0; i < length; i++) {
                temp [i] = (char)ReadByte ();
            }
            return temp;
        }

        public int ReadBEInt32 ()
        {
            int b1 = ReadByte ();
            int b2 = ReadByte ();
            int b3 = ReadByte ();
            int b4 = ReadByte ();
            return b4 + (b3 << 8) + (b2 << 16) + (b1 << 24);
        }
        
        public int ReadBEInt16 ()
        {
            int b1 = ReadByte ();
            int b2 = ReadByte ();
            return b2 + (b1 << 8);
        }

        public int ReadMultiByteValue ()
        {
            int value = 0;
            while (true) {
                int b = ReadByte ();
                value += b & 0x7f;
                if (b < 0x80)
                    break;
                value <<= 7;
            }
            return value;
        }
    }
}
