using System.Diagnostics;
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
        private bool gameOver;
        private Random rnd = new Random();
        double speed = 0;
        int rotater = 1;
        List<Rectangle> pipes = new List<Rectangle>();
        private Stopwatch sw = new Stopwatch();
        private double pipeElapsed = 0;
        private double pipeInterval = 1.5;
        int points_counter = 0;
        public MainWindow()
        {
            InitializeComponent();
            MyCanvas.Visibility = Visibility.Collapsed;
        }
        private void Start_btn_Click(object sender, RoutedEventArgs e)
        {
            gameOver = false;
            StartScreen.Visibility = Visibility.Collapsed;
            MyCanvas.Visibility = Visibility.Visible;
            CompositionTarget.Rendering += GameLoop;
            sw.Start();
        }

        private void GameLoop(object? sender, EventArgs e)
        {
            double time = sw.Elapsed.TotalSeconds;
            sw.Restart();

            pipeElapsed += time;
            if (pipeElapsed >= pipeInterval)
            {
                CreatePipe();
                pipeElapsed = 0;
            }

            if(Collision()) return;
            Move(bird);
        }

        private void Move(Image Bird)
        {
            const double acc = 1.0 / 3.0;
            double pos_now = Canvas.GetTop(Bird);
            speed += acc;
            double new_pos = pos_now + speed;
            Canvas.SetTop(Bird, new_pos);
            MoveBackground();
            birdRotate.Angle = speed * 2 + rotater;
            if (rotater < 1)
            {
                rotater += 20;
            }
            else if (rotater > 1)
            {
                rotater -= 20;
            }

        }

        private void GameOver()
        {
            CompositionTarget.Rendering -= GameLoop;
            MessageBox.Show("VÉGE", "VÉGE");
            MyCanvas.Visibility = Visibility.Collapsed;
            StartScreen.Visibility = Visibility.Visible;
            pipes.Clear();
            MyCanvas.Children.Clear();
            Canvas.SetLeft(bird, 0);
            Canvas.SetLeft(bird, 0);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (rotater == 1)
                {
                    speed = -7.5;

                }
            }
            else if (e.Key == Key.Down)
            {
                rotater = 361;
                speed = speed > 0 ? -1 * speed * 0.5 : speed;
            }
        }

        private void CreatePipe()
        {
            int pipe_y = rnd.Next(50, 200);
            int pipe_x = 1080;
            int pipe_width = 100;
            int pipe_height = 300;
            int pipe_gap = rnd.Next(135, 185);

            Rectangle rect_top = new Rectangle
            {
                Width = pipe_width,
                Height = pipe_height,
                Fill = Brushes.Green
            };
            Rectangle rect_bot = new Rectangle
            {
                Width = pipe_width,
                Height = pipe_height,
                Fill = Brushes.Green
            };
            Rectangle rect_pnt = new Rectangle
            {
                Width = pipe_width,
                Height = pipe_gap,
                Name = "pnt_rect"
            };
            Canvas.SetLeft(rect_pnt, pipe_x);
            Canvas.SetTop(rect_pnt, pipe_y);

            Canvas.SetLeft(rect_top, pipe_x);
            Canvas.SetTop(rect_top, pipe_y - pipe_height);

            Canvas.SetLeft(rect_bot, pipe_x);
            Canvas.SetTop(rect_bot, pipe_y + pipe_gap);

            pipes.Add(rect_pnt);
            pipes.Add(rect_top);
            pipes.Add(rect_bot);

            MyCanvas.Children.Add(rect_pnt);
            MyCanvas.Children.Add(rect_top);
            MyCanvas.Children.Add(rect_bot);
        }

        private bool Collision()
        {
            Rect birdBox = new Rect(Canvas.GetLeft(bird), Canvas.GetTop(bird), bird.ActualWidth, bird.ActualHeight);

            for (int pipe = pipes.Count() - 1; pipe >= 0; pipe--)
            {
                var cso = pipes[pipe];

                Rect pipeBox = new Rect(Canvas.GetLeft(cso), Canvas.GetTop(cso), cso.ActualWidth, cso.ActualHeight);
                pipeBox.Inflate(-7, -7);

                if (birdBox.IntersectsWith(pipeBox) && cso.Name == "pnt_rect")
                {
                    pipes.RemoveAt(pipe);
                    MyCanvas.Children.Remove(cso);
                    points_counter++;
                    points_cnt.Content = points_counter;
                }
                else if (birdBox.IntersectsWith(pipeBox))
                {
                    GameOver();
                    return true;
                }
            }
            PipeCleanup();
            return false;
        }
        private void MoveBackground()
        {
            if (Canvas.GetLeft(layer2) == -layer2.Width) Canvas.SetLeft(layer2, 799);
            else Canvas.SetLeft(layer2, Canvas.GetLeft(layer2) - 0.25);

            if (Canvas.GetLeft(layer22) == -layer22.Width) Canvas.SetLeft(layer22, 799);
            else Canvas.SetLeft(layer22, Canvas.GetLeft(layer22) - 0.25);

            if (Canvas.GetLeft(layer1) == -layer1.Width) Canvas.SetLeft(layer1, 799);
            else Canvas.SetLeft(layer1, Canvas.GetLeft(layer1) - 0.5);

            if (Canvas.GetLeft(layer12) == -layer12.Width) Canvas.SetLeft(layer12, 799);
            else Canvas.SetLeft(layer12, Canvas.GetLeft(layer12) - 0.5);
        }
        private void PipeCleanup()
        {
            for (int pipe = pipes.Count - 1; pipe >= 0; pipe--)
            {
                var cso = pipes[pipe];

                if (Canvas.GetLeft(cso) > -cso.Width)
                {
                    Canvas.SetLeft(cso, Canvas.GetLeft(cso) - 5);
                }
                else
                {
                    MyCanvas.Children.Remove(cso);
                    pipes.RemoveAt(pipe);
                }


            }
        }

    }
}