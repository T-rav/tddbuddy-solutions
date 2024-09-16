// MarsRoverTests.cs
using NUnit.Framework;
using Rover;

namespace MarsRoverTests
{
    public class MarsRoverTests
    {
        [Test]
        public void Given_RoverFacingNorth_When_TurnLeftFourTimes_Then_DirectionsCycleCorrectly()
        {
            // Arrange
            var plateau = new Plateau(5, 5);
            var rover = new MarsRover(0, 0, 'N', plateau);
            var expectedDirections = new[] { 'W', 'S', 'E', 'N' };
            var actualDirections = new char[4];

            // Act
            for (int i = 0; i < 4; i++)
            {
                rover.TurnLeft();
                actualDirections[i] = rover.GetPosition().Direction;
            }

            // Assert
            Assert.That(actualDirections, Is.EqualTo(expectedDirections));
        }

        [Test]
        public void Given_RoverFacingNorth_When_TurnRightFourTimes_Then_DirectionsCycleCorrectly()
        {
            // Arrange
            var plateau = new Plateau(5, 5);
            var rover = new MarsRover(0, 0, 'N', plateau);
            var expectedDirections = new[] { 'E', 'S', 'W', 'N' };
            var actualDirections = new char[4];

            // Act
            for (int i = 0; i < 4; i++)
            {
                rover.TurnRight();
                actualDirections[i] = rover.GetPosition().Direction;
            }

            // Assert
            Assert.That(actualDirections, Is.EqualTo(expectedDirections));
        }

        [Test]
        public void Given_RoverFacingNorthAtOrigin_When_Move_Then_YIncrementsByOne()
        {
            // Arrange
            var plateau = new Plateau(5, 5);
            var rover = new MarsRover(0, 0, 'N', plateau);

            // Act
            rover.Move();
            var position = rover.GetPosition();

            // Assert
            Assert.That(position.X, Is.EqualTo(0));
            Assert.That(position.Y, Is.EqualTo(1));
            Assert.That(position.Direction, Is.EqualTo('N'));
        }

        [Test]
        public void Given_RoverFacingSouthAt0_1_When_Move_Then_YDecrementsByOne()
        {
            // Arrange
            var plateau = new Plateau(5, 5);
            var rover = new MarsRover(0, 1, 'S', plateau);

            // Act
            rover.Move();
            var position = rover.GetPosition();

            // Assert
            Assert.That(position.X, Is.EqualTo(0));
            Assert.That(position.Y, Is.EqualTo(0));
            Assert.That(position.Direction, Is.EqualTo('S'));
        }

        [Test]
        public void Given_RoverFacingEastAtOrigin_When_Move_Then_XIncrementsByOne()
        {
            // Arrange
            var plateau = new Plateau(5, 5);
            var rover = new MarsRover(0, 0, 'E', plateau);

            // Act
            rover.Move();
            var position = rover.GetPosition();

            // Assert
            Assert.That(position.X, Is.EqualTo(1));
            Assert.That(position.Y, Is.EqualTo(0));
            Assert.That(position.Direction, Is.EqualTo('E'));
        }

        [Test]
        public void Given_RoverFacingWestAt1_0_When_Move_Then_XDecrementsByOne()
        {
            // Arrange
            var plateau = new Plateau(5, 5);
            var rover = new MarsRover(1, 0, 'W', plateau);

            // Act
            rover.Move();
            var position = rover.GetPosition();

            // Assert
            Assert.That(position.X, Is.EqualTo(0));
            Assert.That(position.Y, Is.EqualTo(0));
            Assert.That(position.Direction, Is.EqualTo('W'));
        }

        [Test]
        public void Given_RoverAtBoundary_When_MoveBeyond_Then_PositionRemainsSame()
        {
            // Arrange
            var plateau = new Plateau(5, 5);
            var rover = new MarsRover(5, 5, 'N', plateau);

            // Act
            rover.Move();
            var position = rover.GetPosition();

            // Assert
            Assert.That(position.X, Is.EqualTo(5));
            Assert.That(position.Y, Is.EqualTo(5));
            Assert.That(position.Direction, Is.EqualTo('N'));
        }

        [Test]
        public void Given_RoverWithCommands_When_ProcessCommands_Then_FinalPositionIsCorrect()
        {
            // Arrange
            var plateau = new Plateau(5, 5);
            var rover = new MarsRover(1, 2, 'N', plateau);
            var commands = "LMLMLMLMM";

            // Act
            rover.ProcessCommands(commands);
            var position = rover.GetPosition();

            // Assert
            Assert.That(position.X, Is.EqualTo(1));
            Assert.That(position.Y, Is.EqualTo(3));
            Assert.That(position.Direction, Is.EqualTo('N'));
            Assert.That(position.ToString(), Is.EqualTo("1 3 N")); // Optional, to verify the string output
        }

        [Test]
        public void Given_AnotherRoverWithCommands_When_ProcessCommands_Then_FinalPositionIsCorrect()
        {
            // Arrange
            var plateau = new Plateau(5, 5);
            var rover = new MarsRover(3, 3, 'E', plateau);
            var commands = "MMRMMRMRRM";

            // Act
            rover.ProcessCommands(commands);
            var position = rover.GetPosition();

            // Assert
            Assert.That(position.X, Is.EqualTo(5));
            Assert.That(position.Y, Is.EqualTo(1));
            Assert.That(position.Direction, Is.EqualTo('E'));
            Assert.That(position.ToString(), Is.EqualTo("5 1 E")); // Optional, to verify the string output
        }
    }
}
