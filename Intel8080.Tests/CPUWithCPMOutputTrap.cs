using System.Linq;

namespace Intel8080.Tests
{
    internal class CPUWithCPMOutputTrap : CPU
    {
        public delegate void PrintFunc(string s, string terminator = "\n");

        private readonly PrintFunc print;

        public CPUWithCPMOutputTrap(RAM mem, PrintFunc print) : base(mem, Enumerable.Range(0, 8).Select(_ => new Port()).ToList())
        {
            this.print = print;

            mem[5] = 0xc9; // return from output
            
            programCounter = 0x100;
        }

        public override int CALL(byte lo, byte hi)
        {
            var loc = (hi << 8) | lo;
            if (5 == loc)
            {
                if (registers[Registers.Index.C] == 9)
                {
                    var offset = (registers[Registers.Index.D] << 8) | registers[Registers.Index.E];

                    var s = "";
                    char c;
                    while ((c = (char) mem[offset]) != '$')
                    {
                        s += c;
                        offset += 1;
                    }

                    print(s, "");
                }
                else if (registers[Registers.Index.C] == 2)
                {
                    var c = (char) registers[Registers.Index.E];
                    print(c.ToString(), "");
                }
            }

            return base.CALL(lo, hi);
        }
    }
}