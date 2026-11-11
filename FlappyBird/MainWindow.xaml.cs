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
        private bool game = true;
        private DispatcherTimer timer;
        private Random rnd;
        double speed = 0;
        int rotater = 1;
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            Move(bird);

        }

        private void Move(Image Bird)
        {
            const double acc = 0.333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333;
            double pos_now = Canvas.GetTop(Bird);
            speed += acc;
            double new_pos = pos_now + speed;
            Canvas.SetTop(Bird, new_pos);
            birdRotate.Angle = speed + rotater;
            if(rotater < 1)
            {
                rotater += 10;
            }
            else if(rotater > 1)
            {
                rotater -= 10;
            }

        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                speed = -10;
            }
            else if (e.Key == Key.Down)
            {
                rotater = -249;
            }
            else if(e.Key == Key.Up)
            {
                rotater = 251;
            }
        }

        private void CreatePipes()
        {
            while (game)
            {
                int pipe_y_offest = rnd.Next(0, 300);
                int pipe_gap = rnd.Next(20,60);


            }   
        }
    }
}