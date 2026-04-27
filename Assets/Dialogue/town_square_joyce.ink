=== joyce_dialogue ===
{ SpiretonMainQuestState:
    - "REQUIREMENTS_NOT_MET":   -> joyce_before
    - "CAN_START":              -> joyce_before
    - "IN_PROGRESS":            -> joyce_during
    - "CAN_FINISH":             -> joyce_during
    - "FINISHED":               -> joyce_after
    - else:                     -> END
}

= joyce_before
# speaker:Joyce
We're just standing here. I don't love standing here.

+ [We're waiting for the right moment.]
    -> joyce_before_b
+ [Are you ready to go?]
    -> joyce_ready

= joyce_before_b
# speaker:Joyce
The right moment is the one where we stop talking and start moving.

# speaker:Joyce
Talk to the mayor when you're done warming up.
-> END

= joyce_ready
# speaker:Joyce
Have been since before you woke up.

# speaker:Joyce
Talk to the mayor. The sooner we know what we're dealing with, the sooner we move.
-> END

= joyce_during
# speaker:Joyce
We've got a job. Let's not forget that while we're chatting.

+ [Right. Focus.]
    # speaker:Joyce
    Good. Keep that energy.
    -> END
+ [Any concerns before we go?]
    -> joyce_concern

= joyce_concern
# speaker:Joyce
One. We move as a unit or we don't move at all.

# speaker:Joyce
I've seen groups fall apart at the first obstacle because everyone went their own direction.

# speaker:Joyce
Not us. Not today.
-> END

= joyce_after
# speaker:Joyce
We did what we came to do. Linus has a chance.

+ [Couldn't have done it without you.]
    # speaker:Joyce
    You could have. It would have just been uglier.
    -> END
+ [What's next for you?]
    # speaker:Joyce
    Same as always. Find the next thing that needs doing and do it.
    -> END
