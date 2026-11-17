using System;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public partial class PatrolGame : MonoBehaviour
{
    void UpdateCrates()
    {
        selectedCrate=-1;float nearest=1.05f;
        for(int i=0;i<crates.Count;i++) if(!crates[i].opened)
        { float dist=Vector2.Distance(player,crates[i].pos);if(dist<nearest){nearest=dist;selectedCrate=i;} }
        if(selectedCrate>=0 && Press(KeyCode.E))
        {
            Crate c=crates[selectedCrate];c.opened=true;boxSearchCount++;
            if(c.item<0)Say("Empty crate!");
            else { collected[c.item]++; if(c.item==9)ammo+=6; Say("Found: "+itemNames[c.item]); }
        }
    }
}
