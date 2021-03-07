class Enumerable:
    """
    Base class for when you want an Enum class to be able to expose all of its possible values.
    """

    @classmethod
    def tolist(cls):
        """
        Returns all the public attributes of the class.

        :return: list
        """
        return [value for key, value in cls.__dict__.items() if not is_private(key)]


def is_private(key):
    """
    Returns whether or not an attribute is private.
    A private attribute looks like: __private_attribute__.

    :param key: The attribute key
    :return: bool
    """
    return key.startswith("__") and key.endswith("__")
