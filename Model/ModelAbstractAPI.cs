using System;
using Logic;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Numerics;


namespace Model
{
    public abstract class ModelAbstractAPI : IObserver<int>
    {
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(int value);
        public abstract void CreateEllipses(int nrOfBalls);
        public abstract Canvas Canvas { get; set; }
        public abstract List<Ellipse> ellipseCollection { get; }
        public abstract bool IsAnimating { get; set; }

        public static ModelAbstractAPI CreateModelAPI()
        {
            return new ModelAPI();
        }
    }

    public class ModelAPI : ModelAbstractAPI
    {
        private readonly LogicAbstractAPI logicAPI;
        public override List<Ellipse> ellipseCollection { get; }
        public override Canvas Canvas { get; set; }
        private readonly Random random;
        private int ballsCreated = 0;
        public event EventHandler IsAnimatingChanged;
        private readonly IDisposable unsubscriber;


        private bool _isAnimating;
        public override bool IsAnimating
        {
            get { return _isAnimating; }
            set
            {
                if (_isAnimating != value)
                {
                    _isAnimating = value;
                    IsAnimatingChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }


        public ModelAPI()
        {
            logicAPI = LogicAbstractAPI.CreateLogicAPI();
            Canvas = new Canvas();
            ellipseCollection = new List<Ellipse>();
            Canvas = new Canvas
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Width = logicAPI.GetBoardWidth(),
                Height = logicAPI.GetBoardHeight(),
            };
            random = new Random();

            unsubscriber = logicAPI.Subscribe(this);
        }

        public override void OnCompleted()
        {
            unsubscriber.Dispose();
        }

        public override void OnError(Exception error)
        {
            throw new NotImplementedException();
        }


        public override void CreateEllipses(int numberOfBalls)
        {
            logicAPI.CreateBalls(numberOfBalls);

            for (int i = ballsCreated; i < numberOfBalls + ballsCreated; i++)
            {
                SolidColorBrush brush = new SolidColorBrush(Color.FromRgb((byte)random.Next(0, 128), (byte)random.Next(128, 256), (byte)random.Next(128, 256)));
                Ellipse ellipse = new Ellipse
                {
                    Width = logicAPI.GetBallRadius() * 2,
                    Height = logicAPI.GetBallRadius() * 2,
                    Fill = brush
                };

                Vector2 position = logicAPI.GetBallPosition(i);

                double x = position.X + logicAPI.GetBallRadius();
                double y = position.Y - logicAPI.GetBallRadius();

                Canvas.SetLeft(ellipse, x);
                Canvas.SetTop(ellipse, y);

                ellipseCollection.Add(ellipse);
                Canvas.Children.Add(ellipse);
            }

            ballsCreated += numberOfBalls;
        }

        public override void OnNext(int value)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {

                // Aktualizacja pozycji kul na kanwie na podstawie wartości otrzymanej z logiki
                for (int i = 0; i < ellipseCollection.Count; i++)
                {
                    Vector2 ballPosition = logicAPI.GetBallPosition(i);
                    Ellipse ellipse = ellipseCollection[i];

                    Canvas.SetLeft(ellipse, ballPosition.X + logicAPI.GetBallRadius());
                    Canvas.SetTop(ellipse, ballPosition.Y - logicAPI.GetBallRadius());
                }
            });
        }


        public void StartBallAnimation()
        {
            if (!IsAnimating)
            {
                logicAPI.Start();
                IsAnimating = true;
            }
        }

        public void StopBallAnimation()
        {
            if (IsAnimating)
            {
                logicAPI.Stop();
                IsAnimating = false;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Application.Current.Shutdown();
                    Environment.Exit(0);
                });
            }
        }

    }
}