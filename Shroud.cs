
using Il2Cpp;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;

using MelonLoader;

namespace RevealDilemmaMod;

[RegisterTypeInIl2Cpp]
public class Shroud : Demon
{

    public Shroud() : base(ClassInjector.DerivedConstructorPointer<Shroud>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);

    }

    public Shroud(IntPtr ptr) : base(ptr)
    {

    }

    public override string Description
    {
        get => "[After you reveal a character, I deal 1 damage to you. \n\nI Lie and Disguise.]";
    }

    public override void Act(ETriggerPhase trigger, Character charRef)
    {

        if (trigger == ETriggerPhase.Start)
            charRef.statuses.AddStatus(ECharacterStatus.UnkillableByDemon, charRef);

        if (charRef.state == ECharacterState.Dead) return;
    }

    public override CharacterData GetBluffIfAble(Character charRef)
    {
        CharacterData bluff = Characters.Instance.GetRandomUniqueVillagerBluff();
        Gameplay.Instance.AddScriptCharacterIfAble(bluff.type, bluff);
        return bluff;
    }

}