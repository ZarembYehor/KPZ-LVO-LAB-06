using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckersGame.Extensions
{
    public static class ButtonExtensions
    {
        public static int CheckSize;
        public static int GetBoardX(this Button button) 
        {
            return button.Location.X / CheckSize;
        }
        public static int GetBoardY(this Button button)
        {
            return button.Location.Y / CheckSize;
        }
    }
}
