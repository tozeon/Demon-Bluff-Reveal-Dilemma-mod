using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;

namespace RevealDilemmaMod;

public readonly struct CharacterSet
{
    public CharacterSet(CharacterData saboteur, CharacterData shroud, CharacterData auditor)
    {
        Saboteur = saboteur;
        Shroud = shroud;
        Auditor = auditor;
    }

    public CharacterData Saboteur { get; }
    public CharacterData Shroud { get; }
    public CharacterData Auditor { get; }
}

public static class CharacterRegistry
{
    public static void RegisterAll(GameData gameData)
    {
        CharacterSet characters = Register(gameData);
        ConfigureAscensions(gameData, characters);
    }

    public static void injectCharacters()
    {
        ClassInjector.RegisterTypeInIl2Cpp<Saboteur>();
        ClassInjector.RegisterTypeInIl2Cpp<Shroud>();
        ClassInjector.RegisterTypeInIl2Cpp<Auditor>();
    }

    private static CharacterSet Register(GameData gameData)
    {
        Il2CppSystem.Collections.Generic.List<CharacterData> allCharacters = gameData.allCharacterData;

        // Sprite alchemistArt = null;
        // Sprite jesterArt = null;
        // Sprite wretchArt = null;
        // foreach (CharacterData character in allCharacters)
        // {
        //     MelonLogger.Msg(character.characterId);
        //     if (character.characterId == "Alchemist_94446803")
        //     {
        //         alchemistArt = character.art;
        //     }
        //     if (character.characterId == "Wretch_Evil_91222191")
        //     {
        //         wretchArt = character.art;
        //     }
        //     if (character.characterId == "Jester_41367606")
        //     {
        //         jesterArt = character.art;
        //     }
        // }

        CharacterData saboteur = new CharacterData();
        saboteur.role = new Saboteur();
        saboteur.name = "Saboteur";
        saboteur.description = "When you reveal me, I kill the next character you reveal who has an ability.";
        saboteur.flavorText = "\"Looking for any excuse to achieve their goals.\"";
        saboteur.hints = "I can still kill after I die.";
        saboteur.ifLies = "I don't kill or deal damage to you.";
        saboteur.picking = false;
        saboteur.startingAlignment = EAlignment.Good;
        saboteur.type = ECharacterType.Outcast;
        saboteur.abilityUsage = EAbilityUsage.Once;
        saboteur.bluffable = true;
        saboteur.characterId = "Sabo_rdm";
        // saboteur.art = jesterArt;
        saboteur.artBgColor = new Color(0.3679f, 0.2014f, 0.1541f);
        saboteur.cardBgColor = new Color(0.102f, 0.0667f, 0.0392f);
        saboteur.cardBorderColor = new Color(0.7843f, 0.6471f, 0f);
        saboteur.color = new Color(0.9659f, 1f, 0.4472f);

        CharacterData shroud = new CharacterData();
        shroud.role = new Shroud();
        shroud.name = "Shroud";
        shroud.description = "When you reveal a character, I deal 1 damage to you. \n\nI Lie and Disguise.";
        shroud.flavorText = "\"Awaiting in the darkness, the shroud lurks.\"";
        shroud.hints = "";
        shroud.ifLies = "";
        shroud.picking = false;
        shroud.startingAlignment = EAlignment.Evil;
        shroud.type = ECharacterType.Demon;
        shroud.abilityUsage = EAbilityUsage.Once;
        shroud.bluffable = false;
        shroud.characterId = "Shroud_rdm";
        // shroud.art = wretchArt;
        shroud.artBgColor = new Color(1f, 0f, 0f);
        shroud.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
        shroud.cardBorderColor = new Color(0.8208f, 0f, 0.0241f);
        shroud.color = new Color(1f, 0.3811f, 0.3811f);

        CharacterData auditor = new CharacterData();
        auditor.role = new Auditor();
        auditor.name = "Auditor";
        auditor.description = "Pick 2 characters. Heal 2 for each Villager you selected.";
        auditor.flavorText = "\"Verifying the legitimacy of the village.\"";
        auditor.hints = "I Register as a random Outsider.";
        auditor.ifLies = "I never heal you.";
        auditor.picking = true;
        auditor.startingAlignment = EAlignment.Good;
        auditor.type = ECharacterType.Villager;
        auditor.abilityUsage = EAbilityUsage.Once;
        auditor.bluffable = true;
        auditor.characterId = "Auditor_rdm";
        // auditor.art = alchemistArt;
        auditor.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        auditor.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        auditor.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        auditor.color = new Color(1f, 0.935f, 0.7302f);

        allCharacters.Add(shroud);
        allCharacters.Add(saboteur);
        allCharacters.Add(auditor);

        return new CharacterSet(saboteur, shroud, auditor);
    }

    private static void ConfigureAscensions(GameData gameData, CharacterSet characters)
    {
        CharacterData saboteur = characters.Saboteur;
        CharacterData shroud = characters.Shroud;
        CharacterData auditor = characters.Auditor;

        AscensionsData advancedAscension = gameData.advancedAscension;

        CustomScriptData shroudScriptData = new CustomScriptData();
        shroudScriptData.name = "Shroud1";
        ScriptInfo shroudScript = new ScriptInfo();

        Il2CppSystem.Collections.Generic.List<CharacterData> shroudMustInclude = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        shroudMustInclude.Add(shroud);
        shroudMustInclude.Add(auditor);
        shroudScript.mustInclude = shroudMustInclude;

        Il2CppSystem.Collections.Generic.List<CharacterData> shroudDemonList = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        shroudDemonList.Add(shroud);
        shroudScript.startingDemons = shroudDemonList;

        Il2CppSystem.Collections.Generic.List<CharacterData> shroudTownsfolkList = new Il2CppSystem.Collections.Generic.List<CharacterData>(
            gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingTownsfolks.Pointer);
        shroudTownsfolkList.Add(auditor);
        shroudScript.startingTownsfolks = shroudTownsfolkList;
        shroudScript.startingOutsiders = gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingOutsiders;
        shroudScript.startingMinions = gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingMinions;
        // CharactersCount shroudCounter1 = new CharactersCount(7, 4, 1, 1, 1);
        // shroudCounter1.dOuts = shroudCounter1.outs + 1;
        // CharactersCount shroudCounter2 = new CharactersCount(8, 5, 1, 1, 1);
        // shroudCounter2.dOuts = shroudCounter2.outs + 1;
        // CharactersCount shroudCounter3 = new CharactersCount(9, 5, 1, 2, 1);
        // shroudCounter3.dOuts = shroudCounter3.outs + 1;
        CharactersCount shroudCounter4 = new CharactersCount(10, 6, 1, 1, 2);
        shroudCounter4.dOuts = shroudCounter4.outs + 1;
        Il2CppSystem.Collections.Generic.List<CharactersCount> shroudCounterList = new Il2CppSystem.Collections.Generic.List<CharactersCount>();
        // shroudCounterList.Add(shroudCounter1);
        // shroudCounterList.Add(shroudCounter2);
        // shroudCounterList.Add(shroudCounter3);
        shroudCounterList.Add(shroudCounter4);
        shroudScript.characterCounts = shroudCounterList;
        shroudScriptData.scriptInfo = shroudScript;

        Il2CppReferenceArray<CharacterData> advancedAscensionDemons = new Il2CppReferenceArray<CharacterData>(advancedAscension.demons.Length + 1);
        advancedAscensionDemons = advancedAscension.demons;
        advancedAscensionDemons[advancedAscensionDemons.Length - 1] = shroud;
        advancedAscension.demons = advancedAscensionDemons;
        Il2CppReferenceArray<CharacterData> advancedAscensionStartingDemons = new Il2CppReferenceArray<CharacterData>(advancedAscension.startingDemons.Length + 1);
        advancedAscensionStartingDemons = advancedAscension.startingDemons;
        advancedAscensionStartingDemons[advancedAscensionStartingDemons.Length - 1] = shroud;
        advancedAscension.startingDemons = advancedAscensionStartingDemons;
        Il2CppReferenceArray<CustomScriptData> advancedAscensionScriptsData = new Il2CppReferenceArray<CustomScriptData>(advancedAscension.possibleScriptsData.Length + 1);
        advancedAscensionScriptsData = advancedAscension.possibleScriptsData;
        advancedAscensionScriptsData[advancedAscensionScriptsData.Length - 1] = shroudScriptData;
        advancedAscension.possibleScriptsData = advancedAscensionScriptsData;

        foreach (CustomScriptData scriptData in advancedAscension.possibleScriptsData)
        {
            ScriptInfo script = scriptData.scriptInfo;

            AddRole(script.startingTownsfolks, auditor);
            // AddRole(script.startingDemons, shroud);
            AddRole(script.startingOutsiders, saboteur);
        }
    }

    private static void AddRole(Il2CppSystem.Collections.Generic.List<CharacterData> list, CharacterData data)
    {
        if (list.Contains(data))
        {
            return;
        }
        list.Add(data);
    }
}
