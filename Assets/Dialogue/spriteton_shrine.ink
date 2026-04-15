=== Scene_Shrine ===
# minigame:gate_open_missing_ralph

# speaker:Joyce
No. He did not just chase a rabbit here.

# speaker:Aisha
Apparently he did. So we stop hoping he comes to his senses in time and decide what the three of us are doing.

{ roadApproach == "quick_call":
    ~ timeState = "tight"
}

# speaker:Aisha
This gate was built for four.

# speaker:Aisha
Without Ralph, you have to cover both center plates and the lever.

# speaker:Aisha
It can be done. It is just harder.

# speaker:You
What do you set first?

* [Stop and name the three-person fallback before you touch the plates.]
    ~ gateApproach = "planned"
    -> shrine_planned

* [Rush the three-person method before the bloom window slips.]
    ~ gateApproach = "rush"
    ~ timeState = "tight"
    -> shrine_rush


=== shrine_planned ===
# speaker:You
Joyce, west plate. Aisha, east. I take both center plates and the lever. Nobody moves without a call.

{ primaryNorm == "clarify":
    # speaker:Aisha
    Good. Say the order once before we start.
- else:
    { primaryNorm == "help":
        # speaker:Joyce
        Good. If you get stuck between plates, say it out loud.
    - else:
        # speaker:Aisha
        Good. Then nobody has to guess.
    }
}

-> Shrine_OpenThree


=== shrine_rush ===
# speaker:You
Joyce, west. Aisha, east. I take the centers and the lever. Move.

# speaker:Aisha
That is barely a plan.

-> Shrine_OpenThree


=== Shrine_OpenThree ===
{ gateApproach == "planned":
    { leadStyle == "joyce":
        # speaker:Joyce
        My call. Center-west, west, east, center-east, then lever.
    - else:
        # speaker:You
        Center-west, west, east, center-east, then lever. Nobody moves early.
    }
- else:
    # speaker:Joyce
    Center-west, west, east, center-east, then lever. Move.
}

# fx:gate_open
# fx:bloom_collected

-> Shrine_Ralph_Returns


=== Shrine_Ralph_Returns ===
# speaker:Ralph
I saw the rabbit slip under the wall and, for reasons that do not make me look smart, decided that might mean there was another way in.

# speaker:Ralph
Then I lost the rabbit. Then I lost the path.

# speaker:Ralph
Then I lost all of you.

# speaker:Joyce
We know. We just opened a four-person gate with three people because you followed a rabbit.

# speaker:Aisha
And I would like to hear that you understand what that cost us.

# speaker:You
How do you handle it?

* [Name the harm and repair it now.]
    ~ ralphRepair = "repair_now"
    ~ trustState = "repaired"
    -> shrine_repair_now

* [Bury it and keep moving.]
    ~ ralphRepair = "defer"
    ~ trustState = "strained"
    -> shrine_defer

* [Let the anger land hard.]
    ~ ralphRepair = "raw"
    ~ trustState = "strained"
    -> shrine_raw


=== shrine_repair_now ===
{ primaryNorm == "respect":
    # speaker:You
    We said we would call the problem without turning on each other.

    # speaker:You
    You left us to force that gate without you.

    # speaker:You
    Start there. Then tell us what changes.
- else:
    { primaryNorm == "help":
        # speaker:You
        You left us to force that gate without you.

        # speaker:You
        Start there. Then tell us how you stay with us from here.
    - else:
        # speaker:You
        You left us to force that gate without you.

        # speaker:You
        Start there. Then tell us what changes.
    }
}

# speaker:Ralph
I left you short at a gate built for four because I followed a rabbit like it had a plan.

# speaker:Ralph
From here on, I ask before I peel off, and I stay where someone can see me.

# speaker:Joyce
Good. Then you stay on my left and you answer when I call your name.

# speaker:Aisha
That is specific enough to trust.

-> Scene_Lab


=== shrine_defer ===
# speaker:You
We are not doing this here. Take your place and stay where we can see you.

# speaker:Ralph
Right. Yeah. I can do that.

# speaker:Joyce
Good. Then do it.

-> Scene_Lab


=== shrine_raw ===
# speaker:You
We had to force your gate without you. That does not get brushed off.

# speaker:Ralph
I know.

# speaker:Aisha
Then stand where we can all see you and keep moving.

-> Scene_Lab
