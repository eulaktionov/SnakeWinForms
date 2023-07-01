using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeWF
{
    internal class Game : Panel
    {
        public static int size = 40;
        int side = 20;
        Color backColor = Color.Lime;
        Color snakeColor = Color.Red;
        Color foodColor = Color.Yellow;
        int penWidth = 3;

        Random random = new Random();
        Place RandomPlace => new()
        {
            Col = random.Next(side),
            Row = random.Next(side)
        };

        Snake snake;
        Cell food;

        public Game()
        {
            Size = new Size(size * side + penWidth, 
                size * side + penWidth);
            BackColor = backColor;
            //DrawLines();

            food = CreateFood();
            Controls.Add(food);

            snake = CreateSnake();
            foreach (var item in snake.Body)
            {
                Controls.Add(item);
            }

            Paint += (s, e) => DrawLines();
        }

        void DrawLines()
        {
            Graphics graphics = CreateGraphics();
            Pen pen = new Pen(Color.Black, penWidth);
            for(int i = 0; i <= side; i++)
            {
                int y = i * size;
                graphics.DrawLine(pen,
                    new Point(0, y), new Point(side * size, y));
            }
            for(int i = 0; i <= side; i++)
            {
                int x = i * size;
                graphics.DrawLine(pen,
                    new Point(x, 0), new Point(x, side * size));
            }
        }

        Cell CreateFood() => new Cell()
        {
            Place = RandomPlace,
            BackColor = foodColor
        };

        Snake CreateSnake() => new Snake()
        {
            Place = RandomPlace,
            Color = snakeColor
        };


    }

    internal class Snake
    {
        public List<Cell> Body;
        public Place Place
        { 
            get => Body[0].Place; 
            set => Body[0].Place = value; 
        }
        public Color Color
        {
            get => Body[0].BackColor;
            set => Body[0].BackColor = value;
        }
        public Snake()
        {
            Body = new();
            Body.Add(new Cell());
        }
    }

    internal class Cell : PictureBox
    {
        public Place Place 
        {
            get => new Place()
            {
                Row = (int)Top/Game.size,
                Col = Left/Game.size
            };
            set 
            {
                Top = value.Row * Game.size;
                Left = value.Col * Game.size;
            }
        }
        public Cell()
        {
            Width = Game.size;
            Height = Game.size;
        }
    }

    internal class Place
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public static bool operator ==(Place place1, Place place2)
            => place1.Col == place2.Col && place1.Row == place2.Row;
        public static bool operator !=(Place place1, Place place2)
            => !(place1 == place2);
    }
}
