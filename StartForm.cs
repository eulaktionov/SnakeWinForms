namespace SnakeWF
{
    public partial class StartForm : Form
    {
        int panelHeight = 40;

        Panel panel;
        Label points;
        Button button;
        Game game;

        public StartForm()
        {
            InitializeComponent();

            panel = CreatePanel();
            Controls.Add(panel);

            game = new Game(ShowPoints);
            game.TurnOn += () => button.Enabled = true;
            Controls.Add(game);
            ClientSize = new Size(game.Width, panel.Height + game.Height);
            StartPosition = FormStartPosition.CenterScreen;
            Load += (s, e) => Start();
            KeyDown += (s, e) => SetDirection(e.KeyCode);
        }

        void SetDirection(Keys key)
        {
            if (key == Keys.Left ||
                key == Keys.Up ||
                key == Keys.Right ||
                key == Keys.Down)
            {
                game.Direction = key;
            }
        }

        void Start()
        {
            Text = "Snake";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
        }

        Panel CreatePanel()
        {
            Panel panel = new()
            {
                Height = panelHeight,
                Dock = DockStyle.Top
            };
            Label label = new()
            {
                Text = "Points: ",
                Font = new Font(Font.FontFamily, 12)
            };
            panel.Controls.Add(label);

            points = new()
            {
                Left = label.Right,
                Text = "0",
                Font = new Font(Font.FontFamily, 12)
            };
            panel.Controls.Add(points);

            button = new()
            {
                Left = points.Right,
                Width = 120,
                Height = panelHeight,
                Text = "New game",
                Enabled = false
            };
            button.Click += (s, e) => Restart();
            panel.Controls.Add(button);

            return panel;
        }

        private void StartForm_Click(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void ShowPoints(int points)
        {
            this.points.Text = points.ToString();
        }

        void Restart()
        {
            points.Text = "0";
            button.Enabled = false;
            game.Restart();
        }
    }
}
/*
switch(key)
{
case 1:
    item = 1;
    break;
case 1:
    item = 1;
    break;
}

item = switch(key)
{
1=>key, 2=> key, _item
}
*/