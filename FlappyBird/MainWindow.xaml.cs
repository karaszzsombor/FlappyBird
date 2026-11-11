using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FlappyBird
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private DispatcherTimer timerpipe;
        private Random rnd = new Random();
        double speed = 0;
        int rotater = 1;
        List<Rectangle> pipes = new List<Rectangle>();
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            timerpipe = new DispatcherTimer();
            timerpipe.Interval = TimeSpan.FromMilliseconds(2000);
            timerpipe.Tick += Timerpipe_Tick;
            timerpipe.Start();
        }

        private void Timerpipe_Tick(object? sender, EventArgs e)
        {
            CreatePipe();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (!Collision())
            {
                Move(bird);
            }

        }

        private void Move(Image Bird)
        {
            const double acc = 0.333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333;
            double pos_now = Canvas.GetTop(Bird);
            speed += acc;
            double new_pos = pos_now + speed;
            Canvas.SetTop(Bird, new_pos);
            birdRotate.Angle = speed*2+rotater;
            if(rotater < 1)
            {
                rotater += 10;
            }
            else if(rotater > 1)
            {
                rotater -= 10;
            }

            for(int pipe = 0; pipe < pipes.Count(); pipe++)
            {
                var cso = pipes[pipe];

                if (Canvas.GetLeft(cso) > -cso.Width)
                {
                    Canvas.SetLeft(cso, Canvas.GetLeft(cso) - 5);
                }
                else
                {
                    pipes.RemoveAt(pipe);
                }
            }

        }

        private void GameOver()
        {
            MessageBox.Show("VÉGE", "VÉGE");
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                if (rotater == 1)
                {
                    speed = -10;

                }
            }
            else if (e.Key == Key.Down)
            {
                rotater = 361;
                speed = speed > 0 ? speed * -0.6 : speed * 0.6;
            }
            else if(e.Key == Key.Up)
            {
                rotater = -359;
                speed += 4;
            }
        }

        private void CreatePipe()
        {
            int pipe_y = rnd.Next(100, 130);
            int pipe_x = 1080;
            int pipe_gap = rnd.Next(200, 230);

            Rectangle rect_top = new Rectangle
            {
                Width = 100,
                Height = 300,
                Fill = Brushes.Green
            };
            Rectangle rect_bot = new Rectangle
            {
                Width = rect_top.Width,
                Height = rect_top.Height,
                Fill = Brushes.Green
            };
            Canvas.SetLeft(rect_top, pipe_x);
            Canvas.SetTop(rect_top, -pipe_y);

            Canvas.SetLeft(rect_bot, pipe_x);
            Canvas.SetTop(rect_bot, -1*Canvas.GetTop(rect_top)+pipe_gap);

            pipes.Add(rect_top);
            pipes.Add(rect_bot);

            MyCanvas.Children.Add(rect_top);
            MyCanvas.Children.Add(rect_bot);
        }

        private bool Collision()
        {
            for (int pipe = 0; pipe < pipes.Count(); pipe++)
            {
                var cso = pipes[pipe];

                Rect birdBox = new Rect(Canvas.GetLeft(bird), Canvas.GetTop(bird), bird.ActualWidth, bird.ActualHeight);
                Rect pipeBox = new Rect(Canvas.GetLeft(cso), Canvas.GetTop(cso), cso.ActualWidth-5, cso.ActualHeight-5);


                if (birdBox.IntersectsWith(pipeBox))
                {
                    GameOver();
                    return true;
                }
            }
            return false;

        }
    }
}