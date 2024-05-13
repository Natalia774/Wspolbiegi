using System;
using Logic;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Numerics;
using System.Threading;


namespace Model
{
    public abstract class ModelAbstractAPI
    {
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
        private List<(Ellipse, EventHandler)> ballHandlers;
        public event EventHandler IsAnimatingChanged;
        private Dictionary<int, Ellipse> ellipseDictionary = new Dictionary<int, Ellipse>();



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

            ballHandlers = new List<(Ellipse, EventHandler)>();
        }


        public override void CreateEllipses(int numberOfBalls)
        {
            logicAPI.CreateBalls(numberOfBalls);

            for (int i = ballsCreated; i < numberOfBalls + ballsCreated; i++)
            {
                SolidColorBrush brush = new SolidColorBrush(Color.FromRgb((byte)random.Next(128, 256), (byte)random.Next(0, 1), (byte)random.Next(128, 256)));
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
                int ellipseIndex = ellipseCollection.Count - 1;
                ellipseDictionary.Add(ellipseIndex, ellipse);

                Canvas.Children.Add(ellipse);

                EventHandler renderingHandler = CreateRenderingHandler(ellipse, ellipseIndex);
                ballHandlers.Add((ellipse, renderingHandler));
            }

            ballsCreated += numberOfBalls;
        }

        private EventHandler CreateRenderingHandler(Ellipse ellipse, int ellipseIndex)
        {
            return (sender, e) =>
            {
                if (Canvas.Children.Count != 0)
                {
                    Vector2 ballPosition = logicAPI.GetBallPosition(ellipseIndex);
                    Canvas.SetLeft(ellipse, ballPosition.X + logicAPI.GetBallRadius());
                    Canvas.SetTop(ellipse, ballPosition.Y - logicAPI.GetBallRadius());

                    logicAPI.DetectAndHandleCollisions();
                    Thread.Sleep(5);
                }

                else
                {
                    Environment.Exit(0);
                }
            };
        }


        public void StartBallAnimation()
        {
            if (!IsAnimating)
            {
                logicAPI.Start();
                IsAnimating = true;
                foreach (var handler in ballHandlers)
                {
                    CompositionTarget.Rendering += handler.Item2;
                }
            }
        }

        public void StopBallAnimation()
        {
            if (IsAnimating)
            {
                logicAPI.Stop();
                ballHandlers = null;
                ellipseCollection.Clear();
                logicAPI.ClearBalls();
                ellipseDictionary.Clear();
                IsAnimating = false;
                Canvas.Children.Clear();
            }
        }
    }
}