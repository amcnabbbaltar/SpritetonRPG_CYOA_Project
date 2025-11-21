=== npc_Random_Dialogue ===

# speaker:NPC
What can I do for ya?

+ [What are you doing?]
    # speaker:NPC # portrait:smile
    Hangin' around, just enjoying the evening cool.
    -> npc_Random_Dialogue

+ [What’s new in town?]
    # speaker:NPC # portrait:point
    I don't know it's my first day out here!
    -> npc_Random_Dialogue


+ [Can you take me to the team fight area ?]
    # speaker:NPC
    Sure thing, let’s go.
    ~ SwitchScene("TRPG_Exemple")
    -> END

* [That’s all. Thanks!]
    # speaker:NPC
    See you around brotein shake!
    -> END
