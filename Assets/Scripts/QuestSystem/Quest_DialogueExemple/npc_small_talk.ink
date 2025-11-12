=== npcSmallTalk_Menu ===
{ CollectCoinsQuestState != "FINISHED":
    - # speaker:NPC
      Sorry—come back after you’ve wrapped up that coin thing.
      -> END
}
# speaker:NPC
Hey there. Need something?

+ [How are you?]
    # speaker:NPC # portrait:smile
    Can't complain. Happy wife happy life y'know?
    -> npcSmallTalk_Menu

+ [What’s new in town?]
    # speaker:NPC # portrait:point
    A couple of weirdos posted up over there trying to get people to collect coins for them.
    -> npcSmallTalk_Menu

+ [Why are there coins all over the floor?]
    # speaker:NPC
    I honestly don't know, but I wouldn't be complainin' if I were you!
    -> npcSmallTalk_Menu

* [That’s all. Thanks!]
    # speaker:NPC
    See you around bronana peel.
    -> END
