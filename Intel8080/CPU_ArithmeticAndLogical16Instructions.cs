using Intel8080.OperandStrategies;

namespace Intel8080
{
    public partial class CPU
    {
        // "If registers D and E contain 38H and FFH respectively, 
        // the instruction: INX D will cause register D to contain 39H and register E to contain OOH."
        private int INX(IWordOperandStrategy rp)
        {
            var i = rp.Read(this);
            i++;
            rp.Write(this, i);
            return 5;
        }

        // DAD Double add, 16 bit
        // The 16-bit number in the specified register pair is added to the 16-bit number held in the Hand L registers using two's complement arithmetic.
        // The result replaces the contents of the H and L registers
        private int DAD(IWordOperandStrategy rp)
        {
            var rpVal = rp.Read(this);
            var hlVal = registers[RegisterIndexPair.HL];
            var res = hlVal + rpVal;
            registers[RegisterIndexPair.HL] = res & 0xffff;
            Flags = Flags.Set(FlagIndex.Carry, (res & 0x10000) != 0);
            return 10;
        }

        // DCX Decrement Register Pair
        // If register H contains 98H and register L contains OOH, the instruction: DCX H will cause register H to contain 97H and register L to con- tain FFH.
        private int DCX(IWordOperandStrategy rp)
        {
            var i = rp.Read(this);
            i--;
            rp.Write(this, i);
            return 5;
        }
    }
}