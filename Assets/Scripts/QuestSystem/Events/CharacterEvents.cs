using System;

public class CharacterEvents
{
    public event Action<int, PlayerContinuousGridMovement> onActiveCharacterChanged;
    public void ActiveCharacterChanged(int index, PlayerContinuousGridMovement character)
    {
        onActiveCharacterChanged?.Invoke(index, character);
    }
}
