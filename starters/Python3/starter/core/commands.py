from abc import ABC, abstractmethod


class Command(ABC):
    """
    Base class for a command. All commands must implement their custom __str__ method to be able
    to be sent to the game via stdout.
    """

    @abstractmethod
    def __str__(self) -> str:
        pass


class RegisterBotCommand(Command):
    """
    Command used to register your bot before starting a game.
    """

    def __init__(self, name):
        """
        :param name: The name of your bot
        """
        self.name = name

    def __str__(self):
        return f"0:{self.name}"


class ActionCommand(Command):
    """
    Command used to send an Action during the game.
    """

    def __init__(self, tick, action):
        """
        :param tick: The current game tick
        :param action: The Action you want to do
        """
        self.tick = tick
        self.action = action

    def __str__(self):
        return f"{self.tick}:{self.action}"
