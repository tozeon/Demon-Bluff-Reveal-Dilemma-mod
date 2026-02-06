
using Il2Cpp;
using Il2CppFIMSpace.Basics;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using UnityEngine;
using RevealDilemmaMod;
using System.ComponentModel.Design;
using static Il2CppSystem.Globalization.CultureInfo;
using static MelonLoader.MelonLaunchOptions;

namespace RevealDilemmaMod;

[RegisterTypeInIl2Cpp]
public class Saboteur : Role
{

    public Saboteur() : base(ClassInjector.DerivedConstructorPointer<Saboteur>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
        
        // Initialize infoRoles list
        infoRoles = new Il2CppSystem.Collections.Generic.List<Role>();
        infoRoles.Add(new Empath());
        infoRoles.Add(new Scout());
        infoRoles.Add(new Investigator());
        infoRoles.Add(new BountyHunter());
        infoRoles.Add(new Lookout());
        infoRoles.Add(new Knitter());
        infoRoles.Add(new Tracker());
        infoRoles.Add(new Shugenja());
        infoRoles.Add(new Noble());
        infoRoles.Add(new Bishop());
        infoRoles.Add(new Archivist());
        infoRoles.Add(new Acrobat2());
    }

    public Saboteur(System.IntPtr ptr) : base(ptr)
    {

    }

        public override ActedInfo GetInfo(Character charRef)
    {
        return infoRoles[UnityEngine.Random.Range(0, infoRoles.Count)].GetInfo(charRef);
    }

    public override string Description
    {
        get => "[When you reveal me, I kill the next character you reveal who has an ability. I give disguise.";
    }

    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger != ETriggerPhase.Day) return;
        onActed?.Invoke(GetInfo(charRef));
    }

    public override void BluffAct(ETriggerPhase trigger, Character charRef)
    {
        if (trigger != ETriggerPhase.Day) return;
        onActed?.Invoke(GetBluffInfo(charRef));
    }

    public override ActedInfo GetBluffInfo(Character charRef)
    {
        Role role = infoRoles[UnityEngine.Random.Range(0, infoRoles.Count)];
        ActedInfo newInfo = role.GetBluffInfo(charRef);
        return newInfo;
    }

    
    public Il2CppSystem.Collections.Generic.List<Role> infoRoles;
}