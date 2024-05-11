using System.Drawing;
using System.Windows.Forms;

namespace CheckersGame
{
    public class CheckerColorManager
    {
        private Image whiteFigureDefault;
        private Image blackFigureDefault;

        public Image WhiteFigure { get; set; }
        public Image BlackFigure { get; set; }

        public CheckerColorManager(Image whiteFigureDefault, Image blackFigureDefault)
        {
            this.whiteFigureDefault = whiteFigureDefault;
            this.blackFigureDefault = blackFigureDefault;

            WhiteFigure = whiteFigureDefault;
            BlackFigure = blackFigureDefault;
        }

        public void UpdateButtonImage(Button button, int player)
        {
            if (player == 1)
            {
                button.Image = WhiteFigure;
            }
            else if (player == 2)
            {
                button.Image = BlackFigure;
            }
        }
    }
}
