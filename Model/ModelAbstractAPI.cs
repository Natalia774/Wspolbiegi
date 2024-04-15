using System;
using System.Diagnostics;
using Logic;
using Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows;

using System.Windows.Media.Animation;


namespace Model
{
    public abstract class ModelAbstractAPI
    {
        //public abstract void StartSimulation(int nrOfBalls);
        public abstract void CreateEllipses(int nrOfBalls);
        public abstract Canvas Canvas { get; set; }
        public abstract List<Ellipse> ellipseCollection { get; }

        public static ModelAbstractAPI CreateModelAPI()
        {
            return new ModelAPI();
        }
    }

    public class ModelAPI : ModelAbstractAPI
    {
        private readonly LogicAbstractAPI logicAPI;
        private readonly DataAbstractAPI dataAPI;
        public override List<Ellipse> ellipseCollection { get; }
        public override Canvas Canvas { get; set; }
        private readonly Random random;

        private List<Storyboard> ballAnimations;

        public ModelAPI()
        {
            dataAPI = DataAbstractAPI.CreateAPI();
            logicAPI = LogicAbstractAPI.LogicAPIConstruct();
            Canvas = new Canvas();
            ellipseCollection = new List<Ellipse>();
            Canvas.HorizontalAlignment = HorizontalAlignment.Stretch;
            Canvas.VerticalAlignment = VerticalAlignment.Stretch;
            Canvas.Width = 300;
            Canvas.Height = 500;
            random = new Random();

            ballAnimations = new List<Storyboard>();
        }

        public void StartBallAnimation()
        {
            foreach (var ellipse in ellipseCollection)
            {
                double speedX = GetRandomSpeed(); 
                double speedY = GetRandomSpeed(); 

                CompositionTarget.Rendering += (sender, e) =>
                {
                    double newX = Canvas.GetLeft(ellipse) + speedX;
                    double newY = Canvas.GetTop(ellipse) + speedY;

                    // Sprawdź, czy kulka dotknęła krawędzi ramki i jeśli tak, odwróć jej kierunek
                    if (newX >= Canvas.ActualWidth - ellipse.Width || newX <= 0)
                    {
                        speedX *= -1; // Odwróć kierunek w osi X
                    }

                    if (newY >= Canvas.ActualHeight - ellipse.Height || newY <= 0)
                    {
                        speedY *= -1; // Odwróć kierunek w osi Y
                    }

                    // Aktualizuj pozycję kulki
                    Canvas.SetLeft(ellipse, newX);
                    Canvas.SetTop(ellipse, newY);
                };
            }
        }

        private double GetRandomSpeed()
        {
            Random random = new Random();
            return random.NextDouble() * 5 + 1; // Losowa prędkość z zakresu 1-6
        }

        public void StopBallAnimation()
        {
            foreach (var storyboard in ballAnimations)
            {
                storyboard.Pause();
            }
        }
        private bool CheckForOverlap(double x, double y)
        {
            foreach (var existingEllipse in ellipseCollection)
            {
                double existingX = Canvas.GetLeft(existingEllipse);
                double existingY = Canvas.GetTop(existingEllipse);
                if (Math.Abs(existingX - x) < 20 && Math.Abs(existingY - y) < 20)
                {
                    return true;
                }
            }
            return false;
        }

        public override void CreateEllipses(int numberOfBalls)
        {
            logicAPI.StartGame(numberOfBalls);

            for (int i = 0; i < numberOfBalls; i++)
            {
                SolidColorBrush brush = new SolidColorBrush(Color.FromRgb((byte)random.Next(0, 128), (byte)random.Next(128, 256), (byte)random.Next(128, 256)));
                Ellipse ellipse = new Ellipse
                {
                    Width = 10,
                    Height = 10,
                    Fill = brush
                };

                double x = random.Next(0, (int)Canvas.Width - 10);
                double y = random.Next(0, (int)Canvas.Height - 10);
                bool isOverlapping = CheckForOverlap(x, y);
                while (isOverlapping)
                {
                    x = random.Next(0, (int)Canvas.Width - 10);
                    y = random.Next(0, (int)Canvas.Height - 10);
                    isOverlapping = CheckForOverlap(x, y);
                }

                Canvas.SetLeft(ellipse, x);
                Canvas.SetTop(ellipse, y);

                ellipseCollection.Add(ellipse);
                Canvas.Children.Add(ellipse);
            }
        }

        

    }
}