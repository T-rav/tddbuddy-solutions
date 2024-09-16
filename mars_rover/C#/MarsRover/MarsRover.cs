using System;

namespace Rover
{
    public class Plateau
    {
        public int Width { get; }
        public int Height { get; }

        public Plateau(int width, int height)
        {
            if (width < 0 || height < 0)
                throw new ArgumentException("Plateau dimensions must be non-negative.");
            Width = width;
            Height = height;
        }

        public bool IsWithinBounds(int x, int y)
        {
            return x >= 0 && x <= Width && y >= 0 && y <= Height;
        }
    }

    public class Position
    {
        public int X { get; }
        public int Y { get; }
        public char Direction { get; }

        public Position(int x, int y, char direction)
        {
            X = x;
            Y = y;
            Direction = direction;
        }

        public override string ToString()
        {
            return $"{X} {Y} {Direction}";
        }
    }

    public class MarsRover
    {
        private int _x;
        private int _y;
        private char _direction;
        private readonly Plateau _plateau;

        private readonly char[] _directions = { 'N', 'E', 'S', 'W' };

        public MarsRover(int x, int y, char direction, Plateau plateau)
        {
            if (!plateau.IsWithinBounds(x, y))
                throw new ArgumentException("Initial position is out of plateau bounds.");

            if (Array.IndexOf(_directions, direction) == -1)
                throw new ArgumentException("Invalid initial direction.");

            _x = x;
            _y = y;
            _direction = direction;
            _plateau = plateau;
        }

        public Position GetPosition()
        {
            return new Position(_x, _y, _direction);
        }

        public void ProcessCommands(string commands)
        {
            foreach (var command in commands)
            {
                switch (command)
                {
                    case 'L':
                        TurnLeft();
                        break;
                    case 'R':
                        TurnRight();
                        break;
                    case 'M':
                        Move();
                        break;
                    default:
                        throw new ArgumentException($"Invalid command: {command}");
                }
            }
        }

        public void TurnLeft()
        {
            var index = (Array.IndexOf(_directions, _direction) + 3) % 4;
            _direction = _directions[index];
        }

        public void TurnRight()
        {
            var index = (Array.IndexOf(_directions, _direction) + 1) % 4;
            _direction = _directions[index];
        }

        public void Move()
        {
            int newX = _x, newY = _y;

            switch (_direction)
            {
                case 'N':
                    newY += 1;
                    break;
                case 'E':
                    newX += 1;
                    break;
                case 'S':
                    newY -= 1;
                    break;
                case 'W':
                    newX -= 1;
                    break;
            }

            if (_plateau.IsWithinBounds(newX, newY))
            {
                _x = newX;
                _y = newY;
            }
            else
            {
                // Optionally, handle the boundary case (e.g., throw an exception or ignore the move)
            }
        }
    }
}
