=== guard_dialogue ===
{ SpiretonMainQuestState:
    - "REQUIREMENTS_NOT_MET":   -> guard_push
    - "CAN_START":              -> guard_push
    - "IN_PROGRESS":            -> guard_during
    - "CAN_FINISH":             -> guard_during
    - "FINISHED":               -> guard_after
    - else:                     -> END
}

= guard_push
# speaker:Guard
Hey. You. The mayor's been waiting on you since dawn.

+ [We were just getting ready.]
    -> guard_push_b
+ [What does he want?]
    -> guard_what
+ [We'll get there when we get there.]
    -> guard_pushback

= guard_push_b
# speaker:Guard
Getting ready takes a minute. Leaving your uncle to die takes a day.

# speaker:Guard
Go talk to the mayor.
-> END

= guard_what
# speaker:Guard
Not my place to say. His place to say.

# speaker:Guard
Which is why you should be over there talking to him instead of over here talking to me.
-> END

= guard_pushback
# speaker:Guard
Your call. But the bloom only opens at noon and the mayor knows where the shrine is.

# speaker:Guard
Just saying.
-> END

= guard_during
# speaker:Guard
Good. You're moving. That's what I like to see.

+ [Any advice for the road?]
    -> guard_advice
+ [Keep the square safe while we're gone.]
    -> guard_safe

= guard_advice
# speaker:Guard
Stay together. The road past the cart gets narrow and people make bad decisions when they're in a hurry.

# speaker:Guard
You look like people who might be in a hurry.
-> END

= guard_safe
# speaker:Guard
That's what they pay me for.

# speaker:Guard
Now go. Clock's running.
-> END

= guard_after
# speaker:Guard
You made it back. Good.

+ [Did you doubt us?]
    -> guard_doubt
+ [Quiet day here?]
    -> guard_quiet

= guard_doubt
# speaker:Guard
Little bit, yeah. No offense.

# speaker:Guard
You had that look people get right before things go sideways.

# speaker:Guard
Glad I was wrong. Now get moving — the road to Redwillow isn't going to walk itself.

~ SwitchScene("TRPG_Exemple")
-> END

= guard_quiet
# speaker:Guard
Blessedly. Exactly how I like it.

# speaker:Guard
Get some rest. You look like you earned it. Go on, head out.

~ SwitchScene("TRPG_Exemple")
-> END
