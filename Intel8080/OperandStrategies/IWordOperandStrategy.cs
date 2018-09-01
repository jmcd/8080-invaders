namespace Intel8080.OperandStrategies
{
    public interface IWordOperandStrategy
    {
        int Read(CPU cpu);
        void Write(CPU cpu, int val);
    }
}