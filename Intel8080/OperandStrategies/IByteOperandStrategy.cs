namespace Intel8080.OperandStrategies
{
    public interface IByteOperandStrategy
    {
        byte Read(CPU cpu);
        void Write(CPU cpu, byte val);
    }
}