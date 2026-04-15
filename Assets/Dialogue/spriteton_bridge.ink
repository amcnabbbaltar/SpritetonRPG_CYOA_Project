=== Scene_Bridge ===
# minigame:bridge_cross

# speaker:Aisha
That line is the whole bridge.

# speaker:Aisha
Someone strong has to take it across, set it on the far ring, and haul it tight before the vial crosses.

{ ralphRepair == "repair_now":
    # speaker:Ralph
    Give me the line and the ring. I can do that.
- else:
    { ralphRepair == "defer":
        # speaker:Ralph
        I know I am not anybody's first choice right now. Still, if this needs hauling, that should probably be me.
    - else:
        # speaker:Ralph
        If this is the heavy part, it should be me.
    }
}

# speaker:Joyce
I can take the vial and get us across myself. Fastest way.

# speaker:Aisha
Fastest until the bridge sways under the vial.

# speaker:Aisha
Ralph is the strongest one here.

# speaker:Aisha
The problem is not whether he can do it.

# speaker:Joyce
No. The problem is that he followed a rabbit an hour ago.

# speaker:You
Who takes the line?

* [Put Ralph on the line.]
    ~ bridgeApproach = "ralph_on_line"
    -> bridge_ralph_on_line

* [Keep Ralph off the line and send Joyce first.]
    ~ bridgeApproach = "keep_ralph_off_line"
    -> bridge_keep_ralph_off


=== bridge_ralph_on_line ===
# speaker:You
Ralph takes the line. Straight across, ring first, then haul. Joyce waits with the vial until Aisha says the bridge is steady.

# speaker:Ralph
Ring first. Then haul. I can do that.

# speaker:Aisha
Good. Joyce crosses only after the rope is tight.

# speaker:Joyce
Fine. Go straight and do not improvise.

# fx:bridge_repaired

-> Scene_Delivery_Reflection


=== bridge_keep_ralph_off ===
# speaker:You
Joyce goes first with the vial. Ralph stays clear until she is over.

# speaker:Ralph
Right. Staying clear.

# speaker:Aisha
That means the bridge stays loose under the vial.

# speaker:Joyce
Ralph. Rope. Now.

# speaker:Ralph
Keep the vial high. Put your weight on me. We go together.

# speaker:Aisha
Yes. That should have been the first plan.

# fx:bridge_repaired

-> Scene_Delivery_Reflection
