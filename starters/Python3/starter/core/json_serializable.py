import json
from abc import ABC


class JSONSerializable(ABC):
    """
    Base class for when you want an object to be serializable in a json format.
    """

    def __str__(self):
        """
        Returns the json string representation of the object.

        :return: str
        """
        return json.dumps(self, default=lambda me: me.__get_dict__(), sort_keys=True)

    def __get_dict__(self):
        """
        Gets the dict of the object to use when serializing. You should override __get_dict__ when you have attributes
        that are not json serializable and that you do not own.

        :return: dict
        """
        return self.__dict__
