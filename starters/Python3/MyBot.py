import json
import random

# Standard output can ONLY BE USED to communicate with the bomberjam process
# Use text files if you need to log something for debugging

# 1) You must send an alphanumerical name up to 32 characters, prefixed by "0:"
# No spaces or special characters are allowed
print("0:MyName" + str(random.randint(1000, 9999)), flush=True)

# 2) Receive your player ID from the standard input
player_id = input()
try:
    int(player_id)
except ValueError as ex:
    raise Exception("Expected an integer player ID but got %s " % player_id) from ex

while True:
    # 3) Each tick, you'll receive the current game state serialized as JSON
    # From this moment, you have a limited time to send an action back to the bomberjam process through stdout
    state = json.loads(input())

    # 4) Send your action prefixed by the current tick number and a colon
    random_action = random.choice(["up", "down", "left", "right", "stay", "bomb"])
    print(str(state['tick']) + ":" + random_action, flush=True)
