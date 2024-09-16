class Plateau:
    def __init__(self, max_x, max_y):
        self.min_x = 0
        self.min_y = 0
        self.max_x = max_x
        self.max_y = max_y

class MarsRover:
    def __init__(self, x, y, direction, plateau):
        self.x = x
        self.y = y
        self.direction = direction  # N, E, S, W
        self.plateau = plateau  # Plateau object for boundary checks

    def turn_left(self):
        directions = ['N', 'W', 'S', 'E']
        idx = directions.index(self.direction)
        self.direction = directions[(idx + 1) % 4]

    def turn_right(self):
        directions = ['N', 'E', 'S', 'W']
        idx = directions.index(self.direction)
        self.direction = directions[(idx + 1) % 4]

    def move(self):
        if self.direction == 'N':
            if self.y + 1 <= self.plateau.max_y:
                self.y += 1
        elif self.direction == 'S':
            if self.y - 1 >= self.plateau.min_y:
                self.y -= 1
        elif self.direction == 'E':
            if self.x + 1 <= self.plateau.max_x:
                self.x += 1
        elif self.direction == 'W':
            if self.x - 1 >= self.plateau.min_x:
                self.x -= 1

    def process_commands(self, commands):
        for command in commands:
            if command == 'L':
                self.turn_left()
            elif command == 'R':
                self.turn_right()
            elif command == 'M':
                self.move()
            else:
                raise ValueError(f"Invalid command: {command}")

    def get_position(self):
        return f"{self.x} {self.y} {self.direction}"
