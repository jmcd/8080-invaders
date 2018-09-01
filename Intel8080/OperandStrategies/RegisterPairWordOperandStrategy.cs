namespace Intel8080.OperandStrategies
{
    public class RegisterPairWordOperandStrategy : IWordOperandStrategy
    {
        private readonly RegisterIndexPair registerIndexPair;

        public RegisterPairWordOperandStrategy(RegisterIndexPair registerIndex) => this.registerIndexPair = registerIndex;

        public int Read(CPU cpu) => cpu.registers[registerIndexPair];

        public void Write(CPU cpu, int val) => cpu.registers[registerIndexPair] = val;
    }
}