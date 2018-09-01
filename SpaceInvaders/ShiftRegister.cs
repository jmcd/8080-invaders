namespace SpaceInvaders
{
    public class ShiftRegister
    {
        private byte lhs;
        private byte rhs;
        private int offset;

        public void SetOffset(byte b)
        {
            offset = b & 0x07;
        }

        public void Write(byte newLhs)
        {
            rhs = lhs;
            lhs = newLhs;
        }

        public byte Result()
        {
            var v = (lhs << 8) | rhs;
            var result = (byte) (v >> (8 - offset));
            return result;
        }
    }
}