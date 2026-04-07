=== Scene_Start ===
{ SpiretonMainQuestState:
    - "CAN_START":  -> canStart
    - "FINISHED":   -> alreadyLeft
    - else:         -> END
}

= canStart
~ StartQuest(SpiretonMainQuestId)

# speaker:Mayor
Your uncle Linus turned for the worse in the night. He is in Redwillow across the river.

# speaker:Mayor
Luma can brew a Solara Bloom tonic for him, but only if you bring her the flower today. The bloom opens once, at noon, at the shrine. The gate there needs all four of you.

# speaker:Mayor
After Luma brews it, your uncle needs to drink the tonic before sunset if you want its full strength.

# speaker:Joyce
Then we move now.

# speaker:Aisha
We move in a second. If we start this day by rushing past each other, it will only get worse once we are tired.

# speaker:Ralph
She is right. If everybody starts talking at once, I lose the plot and follow the wrong thing.

# speaker:You
Then we name one rule for ourselves before we go. What's going to hold our group together when things start going wrong?

* [Listen all the way through before answering.]
    ~ primaryNorm = "listen"
    # speaker:You
    Then that's the promise. Nobody gets cut off just because we are in a hurry.
    # speaker:Aisha
    Good. Then nobody gets skipped because someone louder is in a hurry.
    # speaker:Joyce
    Fine. Just do not let listening turn into stalling.

* [Clarify what we're doing before we move.]
    ~ primaryNorm = "clarify"
    # speaker:You
    Then we slow down long enough to name the thing we're doing before we rush it.
    # speaker:Ralph
    Good. If I know what the job is, I can stay with it. My side ideas are rarely an improvement.
    # speaker:Aisha
    That keeps us from solving the wrong problem.

* [Ask for help before somebody gets stranded alone.]
    ~ primaryNorm = "help"
    # speaker:You
    Then we ask early, not after somebody is already in trouble.
    # speaker:Joyce
    Ask fast, then. Not after somebody freezes up.
    # speaker:Ralph
    That seems kinder on everyone.

* [Call problems out without turning on each other.]
    ~ primaryNorm = "respect"
    # speaker:You
    Then we say what is wrong without making the person next to us the enemy.
    # speaker:Joyce
    Good. Hit the problem. Not the person.
    # speaker:Aisha
    That keeps the conversation useful.

- -> Start_Lead

= alreadyLeft
# speaker:Mayor
Good luck out there. Linus is counting on you.
-> END


=== Start_Lead ===
The east road to the shrine is open. Nobody expects another calm pause once you take it.

# speaker:Joyce
Do we want one team leader? I can do it, but all of you will have to listen when things start going wrong.

# speaker:Aisha
Or we can just work it out together as we go along.

* [Keep leadership shared.]
    ~ leadStyle = "shared"
    # speaker:You
    We keep it shared unless we start tripping over each other. Aisha speaks up if she sees a risk. Joyce makes the call if we stall. Ralph, if you get lost, say it.
    # speaker:Joyce
    Fine. If I call it, nobody waits.
    # speaker:Aisha
    Good. Then everybody knows what each person needs to do.
    # speaker:Ralph
    I can do that. Specific is good for me.

* [Let Joyce take point.]
    ~ leadStyle = "joyce"
    # speaker:You
    You take point when the pressure spikes. Aisha speaks up if she sees a risk. Ralph, if you get lost, say it.
    # speaker:Joyce
    Good. That keeps us moving.
    # speaker:Aisha
    Then I speak the second I see the wrong turn.
    # speaker:Ralph
    That actually helps. Unsupervised Ralph has a poor record.

- -> Start_Depart


=== Start_Depart ===
{ leadStyle == "joyce":
    Joyce takes the front without another word. Aisha watches the edges. Ralph stays close enough to hear every call.
- else:
    Joyce still leans forward, but the group stays together around her. Aisha watches the edges. Ralph stays close enough to speak if he gets lost.
}

# speaker:Mayor
Good luck! Linus is counting on you.

~ FinishQuest(SpiretonMainQuestId)
-> END
