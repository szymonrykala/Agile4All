#!/bin/python3

from dataclasses import asdict, dataclass
import json
import sys


STATE_FILE = 'state.json'
STATE_EDIT_FILE = 'edit.json'


@dataclass
class State:
    payload: str
    preloadedState: str

    @classmethod
    def fromState(cls):
        with open(STATE_FILE, 'r') as stream:
            data = json.loads(stream.read())

            return cls(
                data.get('payload'),
                data.get('preloadedState')
            )

    @classmethod
    def fromEditState(cls):
        with open(STATE_EDIT_FILE, 'r') as stream:
            data = json.loads(stream.read())

            return cls(
                json.dumps([]),
                json.dumps(data)
            )



def toEdit():
    state = State.fromState()
    with open(STATE_EDIT_FILE, 'w+') as stream:
        stream.write(state.preloadedState)   


def toState():
    state = State.fromEditState()
    with open(STATE_FILE, 'w+') as stream:
        stream.write(json.dumps(asdict(state)))


if __name__ == '__main__':
    if 'to-edit' in sys.argv:
        toEdit()

    elif 'to-state' in sys.argv:
        toState()

    else:
        print(f"Podaj 'to-edit' albo 'to-state'")