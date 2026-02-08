using Il2Cpp;
using Il2CppInterop.Runtime.Injection;

using MelonLoader;

namespace RevealDilemmaMod;

[RegisterTypeInIl2Cpp]
public class Martyr : Minion  
{

    public Martyr() : base(ClassInjector.DerivedConstructorPointer<Martyr>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public override void Act(ETriggerPhase trigger, Character charRef)
    {

        MelonLogger.Msg("Doing stuff");
    }

    public override CharacterData GetBluffIfAble(Character charRef)
    {
        CharacterData bluff = Characters.Instance.GetRandomDuplicateBluff();
        Gameplay.Instance.AddScriptCharacterIfAble(bluff.type, bluff);
        return bluff;
    }

}