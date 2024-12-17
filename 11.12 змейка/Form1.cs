namespace _11._12_змейка;

public partial class MainForm : Form
{

    private List<Point> snake = new List<Point>();
    private Point food;
    private int score = 0;
    private string direction = "RIGHT";
    private bool gameOver = false;
    private Random random = new Random();
    public MainForm()
    {
        InitializeComponent();
        InitGame();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {

    }

    private void InitGame()
    {
        snake.Clear();
        snake.Add(new Point(20, 20));
        food = GenerateFood();
        direction = "RIGHT";
        score = 0;
        gameOver = false;
        scoreLabel.Text = "Score: 0";
        gameTimer.Enabled = true;
    }

    private Point GenerateFood()
    {
        int x = random.Next(0, (ClientSize.Width-20) / 20) * 20;
        int y = random.Next(0, (ClientSize.Height-20) / 20) * 20;

        return new Point(x, y);
    }

    private void gameTimer_Tick(object sender, EventArgs e)
    {
        if (!gameOver)
        {
            MoveSnake();
            CheckCollision();
            Refresh();
        }
        else
        {
            gameTimer.Enabled = false;
            MessageBox.Show("Game Ovber! Score: " + score);
        }
    }

    private void MoveSnake()
    {
        Point head = snake[0];

        switch (direction)
        {
            case "UP":
                head.Y -= 20;
                break;
            case "DOWN":
                head.Y += 20;
                break;
            case "LEFT":
                head.X -= 20;
                break;
            case "RIGHT":
                head.X += 20;
                break;
        }

        snake.Insert(0, head);

        if(head != food)
        {
            snake.RemoveAt(snake.Count - 1);
        }
        else
        {
            score += 10;
            food = GenerateFood();
            scoreLabel.Text = "Score: " + score;
        }
    }

    private void CheckCollision()
    {
        Point head = snake[0];

        if (head.X < 0 || head.Y < 0 || head.X >= ClientSize.Width-20 || head.Y >= ClientSize.Height-20)
        {
            gameOver = true;
        }

        for(int i = 1;i<snake.Count;i++)
        {
            if(head == snake[i])
            {
                gameOver = true;
            }
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        Graphics g = e.Graphics;

        if(!gameOver)
        {
            foreach(Point segment in snake)
            {
                g.FillRectangle(Brushes.Green, new Rectangle(segment.X, segment.Y, 20, 20));
            }

            g.FillRectangle(Brushes.Red, new Rectangle(food.X, food.Y, 20, 20));
        }
        else
        {
            string gameOverText = "Game Over";
            Font font = new Font("Arial",24,FontStyle.Bold);
            SizeF textSize=g.MeasureString(gameOverText, font);
            PointF location = new PointF(
                (ClientSize.Width - textSize.Width) / 2,
                (ClientSize.Height - textSize.Height) /2
            );

            g.DrawString(gameOverText, font, Brushes.Black, location);
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        switch(e.KeyCode)
        {
            case Keys.Up or Keys.W:
                if (direction != "DOWN") direction = "UP";
                break;
            case Keys.Down or Keys.S:
                if (direction != "UP") direction = "DOWN";
                break;
            case Keys.Left or Keys.A:
                if (direction != "RIGHT") direction = "LEFT";
                break;
            case Keys.Right or Keys.D:
                if (direction != "LEFT") direction = "RIGHT";
                break;
            case Keys.Space:
                if (gameOver) InitGame();
                break;
        }
    }
}
