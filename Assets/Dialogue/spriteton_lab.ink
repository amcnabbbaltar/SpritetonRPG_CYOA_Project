=== Scene_Lab ===
# minigame:brew_tonic

# speaker:Luma
Only one rule matters enough to remember out loud.

# speaker:Luma
If the bloom goes in before the kettle cools, the tonic clouds and weakens.

# speaker:Luma
I need the clean vial and the sealing cork from the back shelf. Watch the kettle while I fetch them.

# speaker:Joyce
Then we do it right, and we do it quickly.

# speaker:Joyce
If we brew it too hot, it weakens.

# speaker:Joyce
If Linus drinks it after sunset, it weakens anyway.

# speaker:Joyce
The steam has dropped. I think it is close enough.

# speaker:Aisha
Wait.

# speaker:Joyce
There is no time. We have to do this now. Put the bloom in.

# speaker:You
What do you do?

* [Ask Aisha what she sees.]
    ~ aishaHeard = true
    -> lab_hear_aisha

* [Back Joyce and pour now.]
    ~ aishaHeard = false
    -> lab_pour_now


=== lab_hear_aisha ===
# speaker:You
Aisha, what are you seeing?

# speaker:Aisha
The steam is lighter, yes, but the kettle is still too hot.

# speaker:Aisha
Give it one more count.

# speaker:Aisha
Not long. Just enough that the bloom does not turn bitter when it hits.

# speaker:Joyce
Fine. Your count, then.

{ primaryNorm == "clarify":
    # speaker:Ralph
    That's the kind of sentence I can actually follow.
}

# fx:potion_brewed

-> Scene_Bridge


=== lab_pour_now ===
# speaker:You
Keep going.

# speaker:Joyce
It is close enough. Pour.

# speaker:Luma
It will still help him. It just will not help him as much.

# speaker:Aisha
Then seal it now. Looking at it will not clear it.

# fx:potion_brewed

-> Scene_Bridge
