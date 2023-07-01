using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeWF
{
    internal class Game : Panel
    {
        int size = 40;
        int side = 20;
        Color color = Color.Lime;

        public Game()
        {
            for(int j=0; j < side;  j++)
            {
                for(int i=0; i < side; i++)
                {
                    Controls.Add(new Square()
                    {
                        Location = new Point(i * size, j * size),
                        Size = new Size(size, size),
                        BackColor = color,
                        BorderStyle = BorderStyle.FixedSingle
                    });
                }
            }
            Size = new Size(size * side, size * side);
        }
    }

    internal class Square : Label
    {
        public Square()
        {
            AutoSize = false;
            Text = string.Empty;
        }
    }
}
