
using Il2Cpp;

using Il2CppInterop.Runtime.Injection;

using MelonLoader;

namespace RevealDilemmaMod;

[RegisterTypeInIl2Cpp]
public class Saboteur : Role
{

    public Saboteur() : base(ClassInjector.DerivedConstructorPointer<Saboteur>())
    {
        ClassInjector.DerivedConstructorBody(this);

    }

    // public override ActedInfo GetInfo(Character charRef)
    // {
    //     // return infoRoles[UnityEngine.Random.Range(0, infoRoles.Count)].GetInfo(charRef);
    //     ActedInfo info = new ActedInfo("");
    //     return info;
    // }

    public override void Act(ETriggerPhase trigger, Character charRef)
    {

        if (trigger != ETriggerPhase.Day) return;
        // onActed?.Invoke(GetInfo(charRef));
    }

    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger != ETriggerPhase.Day) return;
        // onActed?.Invoke(GetBluffInfo(charRef));
    }

    // public override ActedInfo GetBluffInfo(Character charRef)
    // {
    //     Role role = infoRoles[UnityEngine.Random.Range(0, infoRoles.Count)];
    //     ActedInfo newInfo = role.GetBluffInfo(charRef);
    //     return newInfo;
    // }


    // public Il2CppSystem.Collections.Generic.List<Role> infoRoles;
}