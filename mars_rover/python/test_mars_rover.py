import pytest
from mars_rover import MarsRover, Plateau

def test_given_rover_facing_north_when_turn_left_four_times_then_directions_cycle_correctly():
    # Arrange
    plateau = Plateau(5, 5)
    rover = MarsRover(0, 0, 'N', plateau)
    expected_directions = ['W', 'S', 'E', 'N']
    # Act
    actual_directions = []
    for _ in range(4):
        rover.turn_left()
        actual_directions.append(rover.direction)
    # Assert
    assert actual_directions == expected_directions

def test_given_rover_facing_north_when_turn_right_four_times_then_directions_cycle_correctly():
    # Arrange
    plateau = Plateau(5, 5)
    rover = MarsRover(0, 0, 'N', plateau)
    expected_directions = ['E', 'S', 'W', 'N']
    # Act
    actual_directions = []
    for _ in range(4):
        rover.turn_right()
        actual_directions.append(rover.direction)
    # Assert
    assert actual_directions == expected_directions

def test_given_rover_facing_north_at_origin_when_move_then_y_increments_by_one():
    # Arrange
    plateau = Plateau(5, 5)
    rover = MarsRover(0, 0, 'N', plateau)
    expected_position = (0, 1)
    # Act
    rover.move()
    actual_position = (rover.x, rover.y)
    # Assert
    assert actual_position == expected_position

def test_given_rover_facing_south_at_0_1_when_move_then_y_decrements_by_one():
    # Arrange
    plateau = Plateau(5, 5)
    rover = MarsRover(0, 1, 'S', plateau)
    expected_position = (0, 0)
    # Act
    rover.move()
    actual_position = (rover.x, rover.y)
    # Assert
    assert actual_position == expected_position

def test_given_rover_facing_east_at_origin_when_move_then_x_increments_by_one():
    # Arrange
    plateau = Plateau(5, 5)
    rover = MarsRover(0, 0, 'E', plateau)
    expected_position = (1, 0)
    # Act
    rover.move()
    actual_position = (rover.x, rover.y)
    # Assert
    assert actual_position == expected_position

def test_given_rover_facing_west_at_1_0_when_move_then_x_decrements_by_one():
    # Arrange
    plateau = Plateau(5, 5)
    rover = MarsRover(1, 0, 'W', plateau)
    expected_position = (0, 0)
    # Act
    rover.move()
    actual_position = (rover.x, rover.y)
    # Assert
    assert actual_position == expected_position

def test_given_rover_at_boundary_when_move_beyond_then_position_remains_same():
    # Arrange
    plateau = Plateau(5, 5)
    rover = MarsRover(5, 5, 'N', plateau)
    expected_position = (5, 5)  # Should not move beyond boundary
    # Act
    rover.move()
    actual_position = (rover.x, rover.y)
    # Assert
    assert actual_position == expected_position

def test_given_rover_with_commands_when_process_commands_then_final_position_is_correct():
    # Arrange
    plateau = Plateau(5, 5)
    rover = MarsRover(1, 2, 'N', plateau)
    expected_position = "1 3 N"
    # Act
    rover.process_commands("LMLMLMLMM")
    actual_position = rover.get_position()
    # Assert
    assert actual_position == expected_position

def test_given_another_rover_with_commands_when_process_commands_then_final_position_is_correct():
    # Arrange
    plateau = Plateau(5, 5)
    rover = MarsRover(3, 3, 'E', plateau)
    expected_position = "5 1 E"
    # Act
    rover.process_commands("MMRMMRMRRM")
    actual_position = rover.get_position()
    # Assert
    assert actual_position == expected_position
