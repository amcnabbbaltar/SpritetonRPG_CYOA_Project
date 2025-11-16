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

+ [You like to play games?]
    # speaker:NPC
    I like to play some games on my phone from time to time, it involves 
    flowers and the undead or something like that.
    -> npc_Random_Dialogue

* [That’s all. Thanks!]
    # speaker:NPC
    See you around brotein shake!
    -> END
