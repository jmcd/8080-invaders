using System;

namespace Intel8080
{
    public class Registers
    {
        public enum Index
        {
            A,
            F,
            B,
            C,
            D,
            E,
            H,
            L
        }

        private readonly byte[] backing;

        public Registers()
        {
            var length = Enum.GetValues(typeof(Index)).Length;
            backing = new byte[length];
        }

        public byte this[Index r]
        {
            get => backing[(int) r];
            set => backing[(int) r] = value;
        }

        public int this[RegisterIndexPair rp]
        {
            get => (backing[(int) rp.LHS] << 8) | backing[(int) rp.RHS];
            set
            {
                var lhs = (byte) (value >> 8);
                var rhs = (byte) value;
                backing[(int) rp.LHS] = lhs;
                backing[(int) rp.RHS] = rhs;
            }
        }
    }
}