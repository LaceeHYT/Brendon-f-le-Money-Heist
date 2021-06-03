using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Brendon_fele_money_heist
{
    public partial class MainWindow : Window
    {

        DispatcherTimer gameTimer = new DispatcherTimer();
        bool moveLeft, moveRight;
        List<Rectangle> itemRemover = new List<Rectangle>();

        Random rand = new Random();

        int ellenfelXarSzamlalo = 0;
        int ellenfelSzamlalo = 100;
        int jatekosSebesseg = 10;
        int limit = 50;
        int pont = 0;
        int sebzes = 0;
        int ellenfelSebesseg = 10;

        Rect jatekosHitBox;

        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

            MyCanvas.Focus();

            ImageBrush bg = new ImageBrush();

            bg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/hatter.png"));
            bg.TileMode = TileMode.Tile;
            bg.Viewport = new Rect(0, 0, 0.15, 0.15);
            bg.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
            MyCanvas.Background = bg;

            ImageBrush jatekosImage = new ImageBrush();
            jatekosImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/jatekos.png"));
            jatekos.Fill = jatekosImage;


        }

        private void GameLoop(object sender, EventArgs e)
        {
            jatekosHitBox = new Rect(Canvas.GetLeft(jatekos), Canvas.GetTop(jatekos), jatekos.Width, jatekos.Height);

            ellenfelSzamlalo -= 1;

            pontText.Content = "Sikeres lopás: " + pont;
            sebzesText.Content = "Elrontott lopás " + sebzes;

            if (ellenfelSzamlalo < 0)
            {
                MakeEnemies();
                ellenfelSzamlalo = limit;
            }

            if (moveLeft == true && Canvas.GetLeft(jatekos) > 0)
            {
                Canvas.SetLeft(jatekos, Canvas.GetLeft(jatekos) - jatekosSebesseg);
            }
            if (moveRight == true && Canvas.GetLeft(jatekos) + 90 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(jatekos, Canvas.GetLeft(jatekos) + jatekosSebesseg);
            }


            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                if (x is Rectangle && (string)x.Tag == "bullet")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 20);

                    Rect bulletHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (Canvas.GetTop(x) < 10)
                    {
                        itemRemover.Add(x);
                    }

                    foreach (var y in MyCanvas.Children.OfType<Rectangle>())
                    {
                        if (y is Rectangle && (string)y.Tag == "Ellenség")
                        {
                            Rect ellenfelSeb = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);

                            if (bulletHitBox.IntersectsWith(ellenfelSeb))
                            {
                                itemRemover.Add(x);
                                itemRemover.Add(y);
                                pont++;
                            }
                        }
                    }

                }

                if (x is Rectangle && (string)x.Tag == "Ellenség")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + ellenfelSebesseg);

                    if (Canvas.GetTop(x) > 750)
                    {
                        itemRemover.Add(x);
                        sebzes += 10;
                    }

                    Rect ellenfelHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (jatekosHitBox.IntersectsWith(ellenfelHitBox))
                    {
                        itemRemover.Add(x);
                        sebzes += 5;
                    }

                }
            }

            foreach (Rectangle i in itemRemover)
            {
                MyCanvas.Children.Remove(i);
            }


            if (pont > 5)
            {
                limit = 20;
                ellenfelSebesseg = 15;
            }

            if (sebzes > 99)
            {
                gameTimer.Stop();
                sebzesText.Content = "Sebzés: 100";
                sebzesText.Foreground = Brushes.Red;
                MessageBox.Show("Bátya, most csak " + pont + " holmit sikerült ellopnod." + Environment.NewLine + "Nyomd meg az Enter-t az újrakezdéshez!");

                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();

            }


        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = true;
            }
            if (e.Key == Key.Right)
            {
                moveRight = true;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = false;
            }
            if (e.Key == Key.Right)
            {
                moveRight = false;
            }

            if (e.Key == Key.Space)
            {
                Rectangle newBullet = new Rectangle
                {
                    Tag = "bullet",
                    Height = 20,
                    Width = 5,
                    Fill = Brushes.White,
                    Stroke = Brushes.Red

                };

                Canvas.SetLeft(newBullet, Canvas.GetLeft(jatekos) + jatekos.Width / 2);
                Canvas.SetTop(newBullet, Canvas.GetTop(jatekos) - newBullet.Height);

                MyCanvas.Children.Add(newBullet);

            }
        }

        private void MakeEnemies()
        {
            ImageBrush ellenfelXar = new ImageBrush();

            ellenfelXarSzamlalo = rand.Next(1, 5);

            switch (ellenfelXarSzamlalo)
            {
                case 1:
                    ellenfelXar.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/1.png"));
                    break;
                case 2:
                    ellenfelXar.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/2.png"));
                    break;
                case 3:
                    ellenfelXar.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/3.png"));
                    break;
                case 4:
                    ellenfelXar.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/4.png"));
                    break;
                case 5:
                    ellenfelXar.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/5.png"));
                    break;
            }

            Rectangle newellenfel = new Rectangle
            {
                Tag = "Ellenség",
                Height = 50,
                Width = 56,
                Fill = ellenfelXar
            };

            Canvas.SetTop(newellenfel, -100);
            Canvas.SetLeft(newellenfel, rand.Next(30, 430));
            MyCanvas.Children.Add(newellenfel);

        }
    }
}