using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bouncing_Balls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Ball> myBalls; //list for all the balls
        public MainWindow()
        {
            InitializeComponent();
            myBalls = new List<Ball>();
        }

        int ballsCounter = 0;

        private void myCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point location = e.GetPosition(myCanvas);
            Random rand = new Random(); //for all the random numbers
            int ballSize = rand.Next(30, 90);
            Ellipse el = new Ellipse(); //for the circle
            el.Height = ballSize;
            el.Width = ballSize;

            el.Fill = new SolidColorBrush(
                Color.FromArgb(
                    (byte)200,
                    (byte)rand.Next(0, 255),
                    (byte)rand.Next(0, 255),
                    (byte)rand.Next(0, 255)));
            myCanvas.Children.Add(el);
            Canvas.SetTop(el, location.Y - el.Height / 2);
            Canvas.SetLeft(el, location.X - el.Width / 2);

            Ball newBall = new Ball //set the new ball
            {
                TheBall = el,
                X = location.X - el.Width / 2,
                Y = location.Y - el.Height / 2,
                XDiff = rand.Next(-10, 11),
                YDiff = rand.Next(-10, 11),
                Radius = el.Height / 2

            };
            tbNumBalls.Text = (++ballsCounter).ToString();
            myBalls.Add(newBall);
            ThreadPool.QueueUserWorkItem(DisplayBall, newBall);
        }

        private void DisplayBall(object obj) //Display Ball
        {

            Ball ball = obj as Ball;

            while (true) //Always runs
            {

                Thread.Sleep(70);  //sleep set the speed of ball movment 

                Dispatcher.Invoke((Action)(() => Canvas.SetTop(
                    ball.TheBall, ball.Y)));
                Dispatcher.Invoke((Action)(() => Canvas.SetLeft(
                      ball.TheBall, ball.X)));

                Dispatcher.Invoke((Action)(() => BorderCheck(ball)));


                ball.X += ball.XDiff; //the next x position (always moving)
                ball.Y += ball.YDiff; //the next x position (always moving)

                double distance = 0; //the distance
                double overlap = 0; //the overlap

                for (int i = 0; i < ballsCounter; i++) // all the balls manegmant 
                {

                    distance = Math.Sqrt(((myBalls[i].X - ball.X) * (myBalls[i].X - ball.X)) + ((myBalls[i].Y - ball.Y) * (myBalls[i].Y - ball.Y))); //d=sqrt((x1-x2)^2 +(y1-y2)^2)
                    if (distance == 0) //distance cant be 0
                        distance = 0.001;

                    Dispatcher.Invoke((Action)(() => overlap = ((ball.Radius) + (myBalls[i].Radius)) - distance));

                    bool inRange = false;
                    Dispatcher.Invoke((Action)(() => inRange = distance <= (ball.Radius) + (myBalls[i].Radius)));

                    if (inRange) // the balls in range
                    {
                        ball.XDiff = ball.XDiff + (ball.X - myBalls[i].X) * (overlap / distance) * 0.3;
                        if (Math.Abs(ball.XDiff) > 15) // check the XDIff in Absolute value
                        {
                            if (ball.XDiff > 0)//if positive
                                ball.XDiff -= 5;
                            else //if not positive
                                ball.XDiff += 5;
                        }
                        ball.YDiff = ball.YDiff + (ball.Y - myBalls[i].Y) * (overlap / distance) * 0.3;
                        if (Math.Abs(ball.YDiff) > 15)// check the YDIff in Absolute value
                        {
                            if (ball.YDiff > 0) //if positive
                                ball.YDiff -= 5;
                            else //if not positive
                                ball.YDiff += 5;
                        }
                    }

                }

            }

        }


        private void BorderCheck(Ball ball) // check if the ball hit the border
        {
            if (ball.X <= 0 || ball.X + ball.TheBall.Width >= myCanvas.ActualWidth) ball.XDiff = -ball.XDiff;
            if (ball.Y <= 0 || ball.Y + ball.TheBall.Height >= myCanvas.ActualHeight) ball.YDiff = -ball.YDiff;
        }

        private void Window_Closed(object sender, EventArgs e) //widow closed
        {
            Environment.Exit(Environment.ExitCode);
        }
    }
}
