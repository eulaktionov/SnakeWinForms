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
            Load += (s, e) => Start();
        }

        void Start()
        {
            Text = "Snake";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(game.Width, game.Height);
        }
    }
}