using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public static class KeyboardMapping
    {
        private static readonly Dictionary<Keys, (int, int)> keyToInputByteAndBit = new Dictionary<Keys, (int, int)>
        {
            {Keys.C, (1, 0)},
            {Keys.Z, (1, 5)},
            {Keys.X, (1, 6)},
            {Keys.Enter, (1, 4)},
            {Keys.D1, (1, 2)},
        };

        public static bool TryGetPortAndBit(Keys keyCode, out (int port, int bit) portBit)
        {
            return keyToInputByteAndBit.TryGetValue(keyCode, out portBit);
        }
    }
}