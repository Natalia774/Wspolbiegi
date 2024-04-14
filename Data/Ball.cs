using System;
using System.Collections.Generic;
using System.Numerics;

namespace Data
{
    public interface IBall
    {
        int R { get; }
        int ID { get; }
        Vector2 Position { get; set; }
        Vector2 Velocity { get; set; }
        float X { get; set; }
        float Y { get; set; }

        void MoveBall();
    }

    internal class Ball : IBall
    {
        private Vector2 velocity;
        private Vector2 position;
        private readonly int r;
        private readonly int id;

        public Ball(Vector2 position, Vector2 velocity, int r, int id)
        {
            this.velocity = velocity;
            this.position = position;
            this.r = r;
            this.id = id;
        }

        public void MoveBall()
        {
            position += velocity; // Używamy prędkości do poruszania kuli
        }

        public float X
        {
            get => position.X;
            set => position.X = value;
        }

        public float Y
        {
            get => position.Y;
            set => position.Y = value;
        }
        public int R => r;

        public int ID => id;

        public Vector2 Position
        {
            get => position;
            set => position = value;
        }

        public Vector2 Velocity
        {
            get => velocity;
            set => velocity = value;
        }
    }
}
