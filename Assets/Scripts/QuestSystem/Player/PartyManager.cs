using System;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [Serializable]
    public class PartyMemberConfig
    {
        public string characterName;
        public Sprite portrait;
        public PlayerContinuousGridMovement controller;
    }

    public static PartyManager instance { get; private set; }

    [SerializeField] private List<PartyMemberConfig> party;

    public int ActiveIndex { get; private set; } = 0;
    public IReadOnlyList<PartyMemberConfig> Party => party;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Party Manager in the scene.");
            return;
        }
        instance = this;

        for (int i = 0; i < party.Count; i++)
            party[i].controller.SetActiveCharacter(i == 0);
    }

    private void Start()
    {
        if (party.Count > 0)
            GameEventsManager.instance.characterEvents.ActiveCharacterChanged(0, party[0].controller);
    }

    public void SwitchToCharacter(int index)
    {
        if (index < 0 || index >= party.Count || index == ActiveIndex) return;

        party[ActiveIndex].controller.SetActiveCharacter(false);
        ActiveIndex = index;
        party[ActiveIndex].controller.SetActiveCharacter(true);

        GameEventsManager.instance.characterEvents.ActiveCharacterChanged(ActiveIndex, party[ActiveIndex].controller);
    }

    public void SwitchToNext()
    {
        SwitchToCharacter((ActiveIndex + 1) % party.Count);
    }
}
