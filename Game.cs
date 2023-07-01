using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.Xml;
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
        int interval = 300;

        Random random = new Random();
        Place RandomPlace => new()
        {
            Col = random.Next(side),
            Row = random.Next(side)
        };

        static bool OutOfRange(int start, int end, int value) =>
            value < start || value > end;

        Snake snake;
        Cell food;
        public Keys Direction { get; set; } = Keys.Left;
        System.Windows.Forms.Timer timer;

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

            timer = new()
            {
                Interval = interval,
                Enabled = true
            };
            timer.Tick += (s,e)=>MoveSnake();

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

        void MoveSnake()
        {
            var snakePlace = snake.Place;
            switch(Direction)
            {
                case Keys.Left: snakePlace.Col--; break;
                case Keys.Up: snakePlace.Row--; break;
                case Keys.Right: snakePlace.Col++; break;
                case Keys.Down: snakePlace.Row++; break;
            }

            if(OutOfGrid(snakePlace))
            {
                EndGame();
                return;
            }

            snake.Place = snakePlace;
        }

        bool OutOfGrid(Place place) =>
            OutOfRange(0, side - 1, place.Col) ||
            OutOfRange(0, side - 1, place.Row);

        void EndGame()
        {
            timer.Stop();
            MessageBox.Show("You lost!");
        }
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
