=== Scene_RoadBlock ===
# speaker:Quin
If this load misses market, I lose the week.

# speaker:Liora
If you wrench that wheel free the wrong way, the bank gives out and we all lose more than a week.

# speaker:Joyce
We can clear a lane and keep moving.

# speaker:Aisha
Or we can spend one minute figuring out whose problem gets worse if we rush.

{ leadStyle == "joyce":
    # speaker:Joyce
    If someone has a better fast answer, now would be a good time.
- else:
    # speaker:Joyce
    Fine. Just make it quick.
}

# speaker:You
What kind of team shows up here?

* [Ask each of them what they need before anyone touches the cart.]
    ~ roadApproach = "listen_first"
    -> roadblock_listen_first

* [Let Joyce call the move before the argument gets worse.]
    ~ roadApproach = "quick_call"
    ~ trustState = "strained"
    -> roadblock_quick_call


=== roadblock_listen_first ===
# speaker:You
One at a time. Quin, what do you need from us?

# speaker:Quin
The cart straight again. That is all.

# speaker:You
Liora, what cannot happen?

# speaker:Liora
That bank cannot give way under us.

# speaker:Aisha
Good. Now we have something we can use.

# speaker:You
Then that is the job. Straighten the cart without kicking the whole road loose.

{ primaryNorm == "listen":
    # speaker:Aisha
    Good. That is exactly why we said it.
}

# fx:road_clear

# speaker:Liora
Thank you for not making this uglier.

-> Scene_Shrine


=== roadblock_quick_call ===
# speaker:You
Joyce, call it.

# speaker:Joyce
Lift on three. Clear the wheel. Keep the stones where they are. We move in one pass.

# speaker:Aisha
That gets us through. It does not make it a good answer.

{ primaryNorm == "respect":
    # speaker:Aisha
    We said problem, not people.
}

# fx:road_clear

# speaker:Quin
It works.

# speaker:Liora
You cleared the cart, but the bank is rougher than I like.

# speaker:Liora
It will hold for now. I am coming back with tools tomorrow.

-> Scene_Shrine
