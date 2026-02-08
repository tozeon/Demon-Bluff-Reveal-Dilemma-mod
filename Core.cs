using Il2Cpp;

using MelonLoader;
using HarmonyLib;

using RevealDilemmaMod;

using static Il2Cpp.GameplayEvents;
using UnityEngine.Analytics;
using Il2CppSirenix.OdinInspector;


[assembly: MelonInfo(typeof(Core), "RevealDilemmaMod", "1.2.0", "tozeon", null)]
[assembly: MelonGame("UmiArt", "Demon Bluff")]

namespace RevealDilemmaMod
{

    public class Core : MelonMod
    {

        // Temp flag variables needed by the mod
        public bool hasSabo = false;
        public bool canSabo = false;
        public Character sabo;
        public bool shroudCanKill = false;
        public bool canAmbush = false;

        public static Core Instance { get; private set; }

        public override void OnInitializeMelon()
        {
            Instance = this;
            CharacterRegistry.injectCharacters();

        }

        public override void OnLateInitializeMelon()
        {

            GameData gameData = ProjectContext.Instance.gameData;
            CharacterRegistry.RegisterAll(gameData);

            OnDeckShuffled += new Action(OnRoundStart);
            GameplayEvents.OnCharacterKilled += new Action<Character>(OnCharacterKilled);
        }

        private void OnRoundStart()
        {
            // ensure flag variables are reset
            hasSabo = false;
            canSabo = false;
            shroudCanKill = false;
            canAmbush = false;
            Il2CppSystem.Collections.Generic.List<Character> allChars = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);

            for (int i = 0; i < allChars.Count; i++)
            {
                CharacterData trueChar = allChars[i].dataRef;
                switch (trueChar.characterId)
                {
                    case "Shroud_rdm":
                        shroudCanKill = true;
                        break;
                    case "Sabo_rdm":
                        sabo = allChars[i];
                        break;
                    case "Ambush_rdm":
                        canAmbush = true;
                        break;
                    default:
                        break;
                }

            }

        }

        private void OnCharacterKilled(Character deadChar)
        {
            switch (deadChar.dataRef.characterId)
            {
                case "Shroud_rdm":
                    shroudCanKill = false;
                    break;
                case "Martyr_rdm":
                    Il2CppSystem.Collections.Generic.List<Character> allChars = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
                    for (int i = 0; i < allChars.Count; i++)
                    {
                        var character = allChars[i];

                        if (character.alignment == EAlignment.Evil && character.state != ECharacterState.Dead && character.dataRef.characterId != "Martyr_rdm")
                        {
                            PlayerController.PlayerInfo.health.Damage(2);
                            break;
                        }
                    }
                    break;
                case "Ambush_rdm":
                    canAmbush = false;
                    break;
            }


        }

    }

    [HarmonyPatch(typeof(Gameplay), "OnCharacterReveal")]
    public static class CharacterRevealPatch
    {

        [HarmonyPrefix]
        public static bool CharacterRevealPrefix(Character obj)
        {
            var core = Core.Instance;
            CharacterData trueChar = obj.dataRef;

            switch (trueChar.characterId)
            {
                case "Sabo_rdm":
                    if (!core.hasSabo)
                    {
                        core.canSabo = true;
                        core.hasSabo = false;
                    }
                    break;
                default: break;
            }

            CharacterData bluffChar = obj.bluff;
            bool criteria = trueChar.picking || (bluffChar != null && bluffChar.picking);
            if (core.canSabo && criteria)
            {
                

                if(obj.alignment != EAlignment.Good) {
                    obj.Kill();
                    return false;
                }

                obj.KillByDemon(core.sabo);

                if (obj.state == ECharacterState.Dead) {
                    PlayerController.PlayerInfo.health.Damage(2);
                }

                core.canSabo = false;
            }

            if (core.shroudCanKill)
            {
                PlayerController.PlayerInfo.health.Damage(1);
            }

            if (!core.canAmbush) return true;
            Il2CppSystem.Collections.Generic.List<Character> allChars = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
            int hidden = allChars.Count;
            for (int i = 0; i < allChars.Count; i++)
            {
                var character = allChars[i];

                if(character.state != ECharacterState.Hidden) {
                    hidden--;
                }
            }
            if(hidden <= 1) {
                PlayerController.PlayerInfo.health.Damage(3);
            }

            return true;

        }
    }
}