using Unity.Netcode;
using UnityEngine;

public class PuzzleManager : NetworkBehaviour
{
    public static PuzzleManager Instance;
    public NetworkVariable<int> puzzle1State = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> puzzle2State = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<bool> puzzle3Complete = new NetworkVariable<bool>(false,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    void Awake() => Instance = this;

    public bool AllComplete() =>
        puzzle1State.Value == 1 && puzzle2State.Value == 1 && puzzle3Complete.Value;
}