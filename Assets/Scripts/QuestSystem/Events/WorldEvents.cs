using System;

public class WorldEvents
{
    public event Action<string, string> onPressurePlateActivated;
    public void PressurePlateActivated(string plateId, string groupId)
    {
        onPressurePlateActivated?.Invoke(plateId, groupId);
    }

    public event Action<string, string> onPressurePlateDeactivated;
    public void PressurePlateDeactivated(string plateId, string groupId)
    {
        onPressurePlateDeactivated?.Invoke(plateId, groupId);
    }

    public event Action<string> onPressurePlateGroupSolved;
    public void PressurePlateGroupSolved(string groupId)
    {
        onPressurePlateGroupSolved?.Invoke(groupId);
    }

    public event Action<string> onPressurePlateGroupReset;
    public void PressurePlateGroupReset(string groupId)
    {
        onPressurePlateGroupReset?.Invoke(groupId);
    }
}
