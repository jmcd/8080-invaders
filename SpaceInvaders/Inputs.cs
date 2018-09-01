namespace SpaceInvaders
{
    public class Inputs
    {
        private readonly byte[] Ports =
        {
            0b00001110,
            0b00001000,
            0b00000000,
        };

        public byte this[int index]
        {
            get => Ports[index];
            set
            {
                if (Ports[index] != value)
                {
#if DEBUG
                    Debug.WriteLine($"p {index} from {Convert.ToString(Ports[index], 2).PadLeft(8, '0')} {Convert.ToString(value, 2).PadLeft(8, '0')}");
                    Debug.WriteLine($"            to {Convert.ToString(value, 2).PadLeft(8, '0')}");
#endif
                    Ports[index] = value;
                }
            }
        }
    }
}