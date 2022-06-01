using System.Drawing;
using System.Windows.Forms;

namespace Assignment1.Drawing
{
    /// <summary>
    /// Square class derives from Winform's Button control class for an ease of access
    /// </summary>
    public class Square : Button
    {
        // constant value for consistency
        public const int SQUARE_SIZE = 25;

        public Square(int x, int y, Color color)
        {
            // customize button
            TabStop = false;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 1;

            // disable the button to make it read only and also apply a back color to this button
            // or at least on theory level, the code does those thing
            BackColor = color;
            Enabled = false;
            Width = SQUARE_SIZE;
            Height = SQUARE_SIZE;

            // using state's x and y position to position the button itself to the screen
            // the origin state is still (0,0), though
            Location = new Point(x * SQUARE_SIZE, y * SQUARE_SIZE);
        }
    
        /// <summary>
        /// Each color defines different square type, as defined in static struct SquareType
        /// </summary>
        /// <param name="color">Color to change to, using SquareType struct or an arbitrary color</param>
        public void ChangeSquareColor(Color color) => BackColor = color;
    }
}