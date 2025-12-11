=== npc_TRPG ===

# speaker:NPC
Hey, you defeat all those enemies yet?

* [Nope.]
    # speaker:NPC # portrait:smile
    Well...get to it!
    -> npc_TRPG

* [What are you doing?]
    # speaker:NPC # portrait:smile
    Hangin' around, just watching you battle.
    -> npc_TRPG

+ [Alright, gotta go.]
    # speaker:NPC
    See ya!
    -> END

