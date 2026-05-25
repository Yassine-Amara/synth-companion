import json, glob
import pandas as pd
import matplotlib.pyplot as plt

all_events = []
for f in glob.glob("session_*.json"):
    with open(f) as file:
        data = json.load(file)
        for e in data["events"]:
            e["sessionId"] = data["sessionId"]
            all_events.append(e)

df = pd.DataFrame(all_events)
print("Actions par joueur:")
print(df.groupby("playerId")["action"].count())

df.groupby("playerId")["action"].count().plot(
    kind="bar", title="Actions par joueur",
    color=["#00d4ff","#29e66a","#ffb800","#ff5a8a"])
plt.tight_layout()
plt.savefig("resultats.png")
plt.show()