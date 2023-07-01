namespace SnakeWF
{
    public partial class StartForm : Form
    {
        Game game;

        public StartForm()
        {
            InitializeComponent();

            game = new Game();
            Controls.Add(game);
            ClientSize = new Size(game.Width, game.Height);
            StartPosition = FormStartPosition.CenterScreen;
            Load += (s, e) => Start();
            KeyDown += (s, e) => game.Direction = e.KeyCode switch
            {
                Keys.Left or Keys.Up or Keys.Right or Keys.Down => e.KeyCode
            };
        }

        void Start()
        {
            Text = "Snake";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
        }
    }
}