
using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using MelonLoader;
using UnityEngine;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace RevealDilemmaMod;

[RegisterTypeInIl2Cpp]
public class Auditor : Role
{

    public Auditor() : base(ClassInjector.DerivedConstructorPointer<Auditor>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);

    }

    public Auditor(IntPtr ptr) : base(ptr)
    {

    }

    public override string Description
    {
        get => "[Pick 2 characters. Heal 2 for each Villager you selected.";
    }

    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger != ETriggerPhase.Day) return;
        CharacterPicker.Instance.StartPickCharacters(2);
        CharacterPicker.OnCharactersPicked = (Il2CppSystem.Action)(() => CharacterPicked(charRef));
        CharacterPicker.OnStopPick = (Il2CppSystem.Action)(() => StopPick());
    }


    private void StopPick()
    {
        CharacterPicker.OnCharactersPicked = null;
        CharacterPicker.OnStopPick = null;
    }

    private void CharacterPicked(Character auditorCharRef)
    {
        CharacterPicker.OnCharactersPicked = null;
        CharacterPicker.OnStopPick = null;

        int count = 0;
        foreach (Character c in CharacterPicker.PickedCharacters)
        {
            if (c != auditorCharRef && c.GetCharacterType() == ECharacterType.Villager)
                count++;
        }

        PlayerController.PlayerInfo.health.Heal(2 * count);

        Il2CppSystem.Collections.Generic.List<Character> chars = CharacterPicker.PickedCharacters;

        string info = AuditorInfo(chars[0].id, chars[1].id, count);
        ActedInfo actedInfo = new ActedInfo(info, chars);
        onActed?.Invoke(actedInfo);
        Debug.Log($"{info}");
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger != ETriggerPhase.Day) return;
        CharacterPicker.Instance.StartPickCharacters(2);
        CharacterPicker.OnCharactersPicked = (Il2CppSystem.Action)(() => CharacterPickedDrunk());
        CharacterPicker.OnStopPick = (Il2CppSystem.Action)(() => StopPick());
    }
    private void CharacterPickedDrunk()
    {
        CharacterPicker.OnCharactersPicked = null;
        CharacterPicker.OnStopPick = null;

        Il2CppSystem.Collections.Generic.List<Character> chars = CharacterPicker.PickedCharacters;

        string info = AuditorInfo(chars[0].id, chars[1].id, 0);
        onActed?.Invoke(new ActedInfo(info, chars));
        Debug.Log($"{info}");
    }

    public string AuditorInfo(int id, int id2, int goodCount)
    {
        string info;
        if (goodCount >= 1)
            info = $"Checking #{id} and #{id2}: I healed {2 * goodCount}";
        else
            info = $"Checking #{id} and #{id2}: I did not heal you";

        return info;
    }

    public override CharacterData GetRegisterAsRole(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<CharacterData> allChars = Gameplay.Instance.GetScriptCharacters();
        allChars = Characters.Instance.FilterCharacterType(allChars, ECharacterType.Outcast);

        if (allChars.Count == 0)
        {
            Il2CppReferenceArray<CharacterData> outcasts = ProjectContext.Instance.gameData.GetStartingtCharactersOfType(ECharacterType.Outcast);
            allChars = new Il2CppSystem.Collections.Generic.List<CharacterData>();
            foreach (CharacterData outcast in outcasts)
            {
                allChars.Add(outcast);
            }
        }

        CharacterData randomOutcast = allChars[UnityEngine.Random.Range(0, allChars.Count)];

        return randomOutcast;
    }

}