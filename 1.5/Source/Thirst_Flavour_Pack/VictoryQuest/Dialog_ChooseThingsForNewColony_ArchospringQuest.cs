using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Thirst_Flavour_Pack.VictoryQuest;

public class Dialog_ChooseThingsForNewColony_ArchospringQuest(
    Action<List<Thing>> postAccepted,
    int maxColonists = 5,
    int maxAnimals = 5,
    int maxRelics = 1,
    int maxItems = 7,
    Action cancel = null)
    : Dialog_ChooseThingsForNewColony(postAccepted, maxColonists, maxAnimals, maxRelics, maxItems, cancel);
