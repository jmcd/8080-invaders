namespace Intel8080.OperandStrategies
{
    public class RegisterByteOperandStrategy : IByteOperandStrategy
    {
        private readonly Registers.Index index;

        public RegisterByteOperandStrategy(Registers.Index index) => this.index = index;

        public byte Read(CPU cpu) => cpu.registers[index];

        public void Write(CPU cpu, byte val) => cpu.registers[index] = val;
    }
}