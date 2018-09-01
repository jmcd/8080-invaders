namespace Intel8080.OperandStrategies
{
    public class MemoryByteOperandStrategy : IByteOperandStrategy
    {
        private static int Location(CPU cpu) => cpu.registers[RegisterIndexPair.HL];

        public byte Read(CPU cpu) => cpu.mem[Location(cpu)];

        public void Write(CPU cpu, byte val) => cpu.mem[Location(cpu)] = val;
    }
}