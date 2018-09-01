using System.Diagnostics.Contracts;

namespace Intel8080 {
    public static class FlagIndexTool
    {
        [Pure]
        public static byte Toggle(this byte flags, FlagIndex flagIndex)
        {
            return flags.Set(FlagIndex.Carry, !flags.IsSet(flagIndex));
        }

        [Pure]
        public static byte Set(this byte flags, FlagIndex flagIndex, bool on)
        {
            var i = (int) flagIndex;
            var flagVal = 1 << i;

            if (@on)
            {
                return (byte) (flags | flagVal);
            }
            return (byte) (flags & (0xff - flagVal));
        }
   
        
        [Pure]
        public static byte calc_SZP(this byte flags, byte value)
        {
            return flags
                    .Set(FlagIndex.Zero, value == 0)
                    .Set(FlagIndex.Sign, (value & 0x80) == 0x80)
                    .Set(FlagIndex.Parity, ParityTable[value] == 1);
            
        }

        public static bool IsSet(this byte flags, FlagIndex flagIndex)
        {
            var i = (int) flagIndex;
            var flagVal = 1 << i;
            return (flags & flagVal) != 0;
        }

        private static readonly byte[] ParityTable =
        {
            1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 0,
            0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 1,
            0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 1,
            1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 0,
            0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 1,
            1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 0,
            1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 0,
            0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 1
        };
    }

    public delegate bool ShouldSetFlag(byte val1, byte val2);

    public static class AuxCarryPredicates
    {
        public static ShouldSetFlag calc_AC =           (val1, val2) => (val1 & 0x0F) + (val2 & 0x0F) > 0x0F;
        public static ShouldSetFlag calc_AC_carry =     (val1, val2) => (val1 & 0x0F) + (val2 & 0x0F) >= 0x0F;
        public static ShouldSetFlag calc_subAC =        (val1, val2) => (val2 & 0x0F) <= (val1 & 0x0F);
        public static ShouldSetFlag calc_subAC_borrow = (val1, val2) => (val2 & 0x0F) < (val1 & 0x0F);
    }
}