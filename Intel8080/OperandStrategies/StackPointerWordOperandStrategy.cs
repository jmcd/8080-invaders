namespace Intel8080.OperandStrategies
{
    public class StackPointerWordOperandStrategy : IWordOperandStrategy
    {
        public int Read(CPU cpu)
        {
            return cpu.stackPointer;
        }

        public void Write(CPU cpu, int val)
        {
            cpu.stackPointer = val;
        }
    }
}