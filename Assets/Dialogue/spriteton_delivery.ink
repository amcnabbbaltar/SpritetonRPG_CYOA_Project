=== Scene_Delivery_Reflection ===
{ aishaHeard == true:
    { timeState == "on_track":
        { bridgeApproach == "ralph_on_line":
            { trustState != "strained":
                ~ outcomeBand = "strong"
            - else:
                ~ outcomeBand = "mixed"
            }
        - else:
            ~ outcomeBand = "mixed"
        }
    - else:
        ~ outcomeBand = "mixed"
    }
- else:
    { trustState == "strained":
        ~ outcomeBand = "fractured"
    - else:
        { bridgeApproach == "keep_ralph_off_line":
            ~ outcomeBand = "fractured"
        - else:
            ~ outcomeBand = "mixed"
        }
    }
}

# speaker:Uncle
You made it.

# speaker:You
We did.

{ outcomeBand == "strong":
    # speaker:Joyce
    Good.

    # speaker:Uncle
    That is much better. Thank you all.
- else:
    { outcomeBand == "mixed":
        # speaker:Aisha
        It is helping. Just not as quickly as we wanted.

        # speaker:Uncle
        Helping is enough for now. Thank you.
    - else:
        # speaker:Aisha
        It still matters that we got here.

        # speaker:Uncle
        It does.
    }
}

# speaker:You
What do you name first?

* [How you held onto each other once the day got hard.]
    ~ reflectionFocus = "trust"
    -> Delivery_Conversation

* [Where the work held, and where it slipped.]
    ~ reflectionFocus = "process"
    -> Delivery_Conversation

* [Where the day sped up on you.]
    ~ reflectionFocus = "timing"
    -> Delivery_Conversation


=== Delivery_Conversation ===
{ reflectionFocus == "trust":
    # speaker:You
    What part of today is still sitting with you?
- else:
    { reflectionFocus == "process":
        # speaker:You
        What part of the work makes the most sense now that it is over?
    - else:
        # speaker:You
        Where did the hurry help, and where did it turn on us?
    }
}

{ ralphRepair == "repair_now":
    # speaker:Ralph
    At the shrine, I came back to a gate the three of you had already fought open without me.

    # speaker:Ralph
    I had earned the anger.

    # speaker:Ralph
    Then at the bridge, you still handed me the rope.
- else:
    { ralphRepair == "defer":
        # speaker:Ralph
        At the shrine, I came back to all of you breathing hard because I had run off after a rabbit.

        # speaker:Ralph
        By the bridge, I could feel nobody wanted that rope in my hands.

        # speaker:Ralph
        I had earned that.
    - else:
        # speaker:Ralph
        At the shrine, I came back to a mess I had made for no good reason.

        # speaker:Ralph
        At the bridge, I still knew the rope was my job, even before anybody wanted to hear it.
    }
}

{ bridgeApproach == "ralph_on_line":
    # speaker:Joyce
    I was still angry, Ralph. I still wanted you on that rope.

    # speaker:Joyce
    Both things were true.

    # speaker:Joyce
    When I stepped onto that bridge, I was trusting you with the worst part of it, and you held.
- else:
    # speaker:Joyce
    I was still angry, Ralph.

    # speaker:Joyce
    I let that choose for me.

    # speaker:Joyce
    Then I was halfway over the river and the person I needed was the one I had kept off the rope.
}

# speaker:Ralph
I know I made that hard.

{ aishaHeard:
    # speaker:Aisha
    The lab held for the same reason the bridge did.

    # speaker:Aisha
    Someone stopped long enough to hear the warning.
- else:
    # speaker:Aisha
    The lab slipped for the same reason the bridge nearly did.

    # speaker:Aisha
    The room decided to move before the warning could land.
}

{ primaryNorm == "clarify":
    # speaker:You
    That started back at the cart.

    # speaker:You
    The minute Quin and Liora named the real problem, we had something we could actually solve.
- else:
    { primaryNorm == "listen":
        # speaker:You
        That started back at the cart.

        # speaker:You
        The minute we really heard both of them, the road stopped being a fight and became a job.
    - else:
        { primaryNorm == "help":
            # speaker:You
            That started back at the cart.

            # speaker:You
            The minute everybody took a part, the road started moving.
        - else:
            # speaker:You
            That started back at the cart.

            # speaker:You
            The minute we stopped treating it like a fight to win, we could work.
        }
    }
}

{ reflectionFocus == "trust":
    # speaker:Aisha
    What saved us was not pretending the shrine did not matter. It was letting one mistake stay one mistake.

    # speaker:You
    Next time we stay honest about the hurt and still make room for the person we need.
- else:
    { reflectionFocus == "process":
        # speaker:Aisha
        The day went best when we matched the job to the person, not the mood to the person.

        # speaker:You
        Next time we name the job first, then the person.
    - else:
        # speaker:Aisha
        The dangerous moments were the ones where hurry started handing out jobs for us.

        # speaker:You
        Next time we make our choices before the clock makes them for us.
    }
}

-> Delivery_Close


=== Delivery_Close ===
# speaker:You
Thank you. For coming with me. For staying when it got hard. He has a chance because of all of you.

# speaker:Joyce
You were never making this trip alone.

# speaker:Aisha
No. That was settled the moment we left the square.

# speaker:Ralph
And next time I plan to be helpful in a way that involves less wildlife.

# speaker:You
Next time, we start there.

# speaker:Joyce
Come on. We can walk home together.

# speaker:Ralph
Middle of the group for me.

# speaker:Aisha
An excellent plan.

# fx:delivery_complete

~ FinishQuest(SpiretonMainQuestId)
-> END
