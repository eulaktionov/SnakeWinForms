using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.Xml;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

using static SnakeWF.Properties.Resources;

namespace SnakeWF
{
    delegate void Show(int points);
    delegate void TurnOn();

    internal class Game : Panel
    {
        public static int size = 40;
        int shift = 3;
        int side = 20;
        Color backColor = Color.Lime;
        //Color snakeColor = Color.Red;
        //Color foodColor = Color.Yellow;
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

        int points;
        Show showPoints;

        public event TurnOn TurnOn;

        public Game(Show showPoints)
        {
            this.showPoints = showPoints;

            Size = new Size(size * side + penWidth, 
                size * side + penWidth);
            BackColor = backColor;

            Cell cell = CreateSnakeCell();
            snake = new(cell)
            {
                Place = RandomPlace
            };
            foreach(var item in snake.Body)
            {
                Controls.Add(item);
            }

            food = new ()
            {
                Image = food01
            };
            MoveFood();
            Controls.Add(food);

            timer = new()
            {
                Interval = interval,
                Enabled = true
            };
            timer.Tick += (s,e)=>MoveSnake();

            Paint += (s, e) => DrawLines();
        }

        public void Restart()
        {
            points = 0;
            RecreateSnake();
            timer.Start();
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

        void MoveFood()
        {
            do
            {
                food.Height = size - shift * 2;
                food.Width = size - shift * 2;
                food.Place = RandomPlace;
                food.Top += shift;
                food.Left += shift;
            }
            while(snake.Include(food));
        }

        Cell CreateSnakeCell()
        {
            Cell cell = new()
            {
                Height = size,
                Width = size
            };

            using(var stream = new MemoryStream(snake01))
            {
                cell.Image = Image.FromStream(stream);
            }
            return cell;
        }

        void RecreateSnake()
        {
            for(int i = snake.Body.Count - 1; i > 0; i--)
            {
                Controls.Remove(snake.Body[i]);
                snake.Body.RemoveAt(i);
            }
            MoveFood();
            snake.Place = RandomPlace;
        }

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
                TurnOn?.Invoke();
                return;
            }

            if(snakePlace == food.Place)
            {
                MoveFood();
                Cell cell = CreateSnakeCell();

                snake.Body.Add(cell);
                Controls.Add(cell);
                showPoints(++points);
            }

            for(int i = snake.Body.Count - 1; i > 0; i--) 
            {
                snake.Body[i].Place = snake.Body[i - 1].Place;
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
        public Snake(Cell header)
        {
            Body = new();
            //Cell cell = new ()
            //{
            //    Height = Game.size,
            //    Width = Game.size
            //};
            //using(var stream = new MemoryStream(snake01))
            //{
            //    cell.Image = Image.FromStream(stream);
            //}
            Body.Add(header);
        }
        public bool Include(Cell cell)
        {
            foreach(var item in Body)
            {
                if(item.Place == cell.Place)
                    return true;
            }
            return false;
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
            SizeMode = PictureBoxSizeMode.StretchImage;
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
