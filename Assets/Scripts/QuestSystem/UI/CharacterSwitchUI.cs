using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitchUI : MonoBehaviour
{
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private CharacterPortraitButton portraitButtonPrefab;

    private readonly List<CharacterPortraitButton> buttons = new List<CharacterPortraitButton>();

    private void OnEnable()
    {
        GameEventsManager.instance.characterEvents.onActiveCharacterChanged += OnActiveCharacterChanged;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.characterEvents.onActiveCharacterChanged -= OnActiveCharacterChanged;
    }

    private void Start()
    {
        BuildUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            PartyManager.instance.SwitchToNext();

        var party = PartyManager.instance.Party;
        for (int i = 0; i < Mathf.Min(4, party.Count); i++)
        {
            if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1 + i)))
                PartyManager.instance.SwitchToCharacter(i);
        }
    }

    private void BuildUI()
    {
        var party = PartyManager.instance.Party;
        for (int i = 0; i < party.Count; i++)
        {
            int capturedIndex = i;
            CharacterPortraitButton btn = Instantiate(portraitButtonPrefab, buttonContainer);
            btn.Setup(
                party[i].characterName,
                party[i].portrait,
                () => PartyManager.instance.SwitchToCharacter(capturedIndex)
            );
            btn.SetSelected(i == PartyManager.instance.ActiveIndex);
            buttons.Add(btn);
        }
    }

    private void OnActiveCharacterChanged(int index, PlayerContinuousGridMovement character)
    {
        for (int i = 0; i < buttons.Count; i++)
            buttons[i].SetSelected(i == index);
    }
}
