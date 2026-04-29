=== joyce_roadblock ===
{ roadBlockResolved:
    -> joyce_roadblock_after
}

{ leadStyle == "joyce":
    -> joyce_roadblock_lead
- else:
    -> joyce_roadblock_shared
}

= joyce_roadblock_lead
# speaker:Joyce
Two people arguing is one person too many. Tell me what you need from me.

* [What is your read on the situation?]
    -> joyce_read
* [Can you clear the cart?]
    -> joyce_clear_plan
* [Wait for us to talk to Quin and Liora first.]
    # speaker:Joyce
    Fine. But be quick about it.
    -> END

= joyce_roadblock_shared
# speaker:Joyce
We can clear a lane and keep moving. What are we waiting for?

* [We want to understand the full problem first.]
    -> joyce_wait
* [What is your plan for the cart?]
    -> joyce_clear_plan
* [Talk to the others before we decide.]
    # speaker:Joyce
    Make it fast.
    -> END

= joyce_read
# speaker:Joyce
One person needs a market delivery. The other is worried about the road edge. Those are not the same problem.

# speaker:Joyce
The cart is the symptom. What they each need is what we have to fix.

-> END

= joyce_clear_plan
# speaker:Joyce
Lift on three. Clear the wheel. Keep the stones where they are. One pass.

# speaker:Joyce
I can call it the moment you are ready.

-> END

= joyce_wait
# speaker:Joyce
Fine. But every minute they argue the job gets worse.
-> END

= joyce_roadblock_after
{ roadApproach == "listen_first":
    # speaker:Joyce
    That took longer than I would have liked. But it held.
- else:
    # speaker:Joyce
    Good. That is what moving looks like.
}
-> END
