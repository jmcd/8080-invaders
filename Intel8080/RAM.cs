namespace Intel8080
{
    public class RAM
    {
        private readonly byte[] backing;

        public RAM(byte[] backing) => this.backing = backing;

        public byte this[int loc]
        {
            get => backing[loc];
            set => backing[loc] = value;
        }
    }
}