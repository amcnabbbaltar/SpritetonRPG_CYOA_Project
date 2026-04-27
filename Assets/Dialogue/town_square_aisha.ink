=== aisha_dialogue ===
{ SpiretonMainQuestState:
    - "REQUIREMENTS_NOT_MET":   -> aisha_before
    - "CAN_START":              -> aisha_before
    - "IN_PROGRESS":            -> aisha_during
    - "CAN_FINISH":             -> aisha_during
    - "FINISHED":               -> aisha_after
    - else:                     -> END
}

= aisha_before
# speaker:Aisha
I've been watching the square. Everyone's a little tense this morning.

+ [Including you?]
    -> aisha_tense
+ [Any idea what the mayor wants?]
    -> aisha_mayor

= aisha_tense
# speaker:Aisha
A little. But tense and ready aren't the same thing.

# speaker:Aisha
I'd rather feel the weight of something before I carry it.
-> END

= aisha_mayor
# speaker:Aisha
The letter mentioned Linus. Beyond that I didn't want to guess.

# speaker:Aisha
Guessing before you have the facts tends to send you in the wrong direction.
-> END

= aisha_during
# speaker:Aisha
We're in the middle of something. What do you need?

+ [Just checking in.]
    -> aisha_checkin
+ [Any worries about the road ahead?]
    -> aisha_worry

= aisha_checkin
# speaker:Aisha
Checking in is good. Most people wait until something's already wrong.

# speaker:Aisha
We're holding together. Keep it that way.
-> END

= aisha_worry
# speaker:Aisha
A few. But worry without a plan is just noise.

# speaker:Aisha
I'm watching. If I see something, I'll say it.
-> END

= aisha_after
# speaker:Aisha
It's strange how much can happen in a single day.

+ [Are you alright?]
    -> aisha_alright
+ [We made the right calls.]
    -> aisha_calls

= aisha_alright
# speaker:Aisha
I think so. Tired. A little glad it's over.

# speaker:Aisha
Ask me again tomorrow. I'll have had time to think about it properly.
-> END

= aisha_calls
# speaker:Aisha
Most of them. The ones we got wrong taught us something too.

# speaker:Aisha
That's usually how it goes.
-> END
