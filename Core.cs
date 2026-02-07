using Il2Cpp;

using MelonLoader;


using RevealDilemmaMod;

using static Il2Cpp.GameplayEvents;


[assembly: MelonInfo(typeof(Core), "RevealDilemmaMod", "1.2.0", "tozeon", null)]
[assembly: MelonGame("UmiArt", "Demon Bluff")]

namespace RevealDilemmaMod;

public class Core : MelonMod
{

    // Temp flag variables needed by the mod
    private bool hasSabo = false;
    private bool canSabo = false;
    private bool shroudCanKill = false;

    public override void OnInitializeMelon()
    {
        CharacterRegistry.injectCharacters();

    }

    public override void OnLateInitializeMelon()
    {

        GameData gameData = ProjectContext.Instance.gameData;
        CharacterRegistry.RegisterAll(gameData);

        GameplayEvents.OnCharacterRevealed += new Action<Character>(OnCharacterRevealed);
        OnDeckShuffled += new Action(OnRoundStart);
        GameplayEvents.OnCharacterKilled += new Action<Character>(OnCharacterKilled);
    }

    private void OnRoundStart()
    {
        MelonLogger.Msg("NEW GAME=================================");
        // ensure flag variables are reset
        hasSabo = false;
        canSabo = false;
        shroudCanKill = false;
        Il2CppSystem.Collections.Generic.List<Character> allChars = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);

        for (int i = 0; i < allChars.Count; i++)
        {
            CharacterData trueChar = allChars[i].dataRef;
            MelonLogger.Msg($"{allChars[i].id}: {allChars[i].dataRef.characterId}");
            switch (trueChar.characterId)
            {
                case "Shroud_rdm":
                    shroudCanKill = true;
                    break;
                default:
                    break;
            }
        }

    }

    private void OnCharacterKilled(Character deadChar)
    {
        if (deadChar.dataRef.characterId == "Shroud_rdm")
        {
            shroudCanKill = false;
        }

    }

    private void OnCharacterRevealed(Character revealedChar)
    {

        CharacterData trueChar = revealedChar.dataRef;
        canSabo = true;
        // shroudCanKill = true;

        switch (trueChar.characterId)
        {
            case "Sabo_rdm":
                if (!hasSabo)
                {
                    canSabo = true;
                    hasSabo = false;
                }
                break;
            default: break;
        }

        CharacterData bluffChar = revealedChar.bluff;
        bool criteria = trueChar.picking || (bluffChar != null && bluffChar.picking);
        if (canSabo && true)
        {
            revealedChar.Kill();

            canSabo = false;
        }

        if (shroudCanKill)
        {
            PlayerController.PlayerInfo.health.Damage(1);
        }

    }


}
