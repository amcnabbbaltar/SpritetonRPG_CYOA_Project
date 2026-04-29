=== Scene_RoadBlock ===
{ SpiretonMainQuestState:
    - "IN_PROGRESS": -> roadblock_intro
    - else: -> END
}

=== roadblock_intro ===
The road narrows above a drop.

A cart has gone sideways across it, one wheel jammed in stone.

Two locals are already blaming each other.

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

-> END


=== roadblock_resolve ===
{ roadBlockResolved:
    -> roadblock_already_resolved
}

* [Ask each of them what they need before anyone touches the cart.]
    ~ roadApproach = "listen_first"
    { spokeTo_Quin && spokeTo_Liora:
        -> roadblock_listen_informed
    - else:
        -> roadblock_listen_first
    }

* [Let Joyce call the move before the argument gets worse.]
    ~ roadApproach = "quick_call"
    -> roadblock_quick_call

= roadblock_already_resolved
The lane is already clear. Keep moving.
-> END


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

Once everyone is talking about the same problem, the fix is simple.

Joyce braces the cart. Quin and Liora guide the wheel clear. Aisha keeps everyone off the soft edge.

The lane opens.

# fx:road_clear

# speaker:Liora
Thank you for not making this uglier.

~ roadBlockResolved = true
~ AdvanceQuest(SpiretonMainQuestId)

=== roadblock_listen_informed ===
# speaker:You
We already know what both of you need. Quin, the cart straight. Liora, the bank intact.

# speaker:You
That is the job. Both at once.

# speaker:Liora
You actually listened.

# speaker:Quin
Then let us get on with it.

# speaker:Aisha
That is the whole thing, right there.

{ primaryNorm == "listen":
    # speaker:Aisha
    That is exactly why we said it.
}

Joyce braces the cart. Quin and Liora guide the wheel clear. Aisha keeps everyone off the soft edge.

The lane opens.

# fx:road_clear

# speaker:Liora
Thank you for not making this uglier.

~ roadBlockResolved = true
~ AdvanceQuest(SpiretonMainQuestId)
~ SwitchScene("TRPG_Exemple")



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

The lane opens quickly, but Quin and Liora are still glaring at each other when you step past.

The cart is through. The argument is not.

# fx:road_clear

# speaker:Quin
It works.

# speaker:Liora
You cleared the cart, but the bank is rougher than I like.

# speaker:Liora
It will hold for now. I am coming back with tools tomorrow.

~ roadBlockResolved = true
~ trustState = "strained"
~ AdvanceQuest(SpiretonMainQuestId)
~ SwitchScene("TRPG_Exemple")


=== roadblock_wheel ===
{ roadBlockResolved:
    The wheel is clear. The road is open.
    -> END
}
{ examined_wheel:
    -> roadblock_wheel_revisit
}

The wheel is jammed in a gap between two flat stones.

The axle is sound. The problem is position, not damage.

A front lift angled toward the road center should let the wheel find its own way clear.

The bank on the left edge has loose fill beneath the surface stone. It will not hold extra weight.

~ examined_wheel = true

* [Note how to free it safely.]
    # speaker:You
    Lift from the front, angle toward center. Keep weight off the left edge.
    -> END

* [File this away for when you are ready.]
    -> END

= roadblock_wheel_revisit
The wheel is still jammed in the stone gap. Front lift, angled center. Left edge stays clear.
-> END


=== roadblock_crate ===
{ roadBlockResolved:
    The crate is already loaded back on the cart.
    -> END
}
{ examined_crate:
    -> roadblock_crate_revisit
}

Grain sacks. Three have split open and are seeping into the road.

There is a market seal on the largest crate. A date stamp from this morning.

Quin was not wrong about the timing. This load is not going to wait.

~ examined_crate = true

* [This explains why Quin is so tense.]
    # speaker:You
    Timed delivery. He does not have room to lose an hour.
    ->roadblock_crate_detroy

* [Good to know.]
    -> END


===roadblock_crate_revisit ===
Grain and a market seal. You already know what is at stake for Quin.
-> END

=== roadblock_crate_detroy ===
~ DestroyNPC()
->END