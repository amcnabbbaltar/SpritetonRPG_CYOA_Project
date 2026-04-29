=== quin_roadblock ===
{ roadBlockResolved:
    -> quin_roadblock_after
}
{ spokeTo_Quin:
    -> quin_roadblock_revisit
}

# speaker:Quin
That wheel is jammed solid. Every minute it sits there I am losing trade.

* [What are you hauling?]
    -> quin_haul
* [How did the cart get stuck?]
    -> quin_stuck
* [We will get this sorted.]
    -> quin_reassure

= quin_haul
# speaker:Quin
Grain. Half a season's worth. If it does not reach market today, I sell it cheap tomorrow or rot it next week.

# speaker:You
Understood. We will keep that in mind.

~ spokeTo_Quin = true
-> END

= quin_stuck
# speaker:Quin
Wheel caught the edge stone on the downgrade. I tried to straighten it and the whole rig went sideways.

# speaker:Quin
Now Liora is in my ear about the bank like I do not already know what a bad bank looks like.

# speaker:You
She is not wrong to worry. But we will find a way through for both of you.

~ spokeTo_Quin = true
-> END

= quin_reassure
# speaker:Quin
Sort it fast, then.

~ spokeTo_Quin = true
-> END

= quin_roadblock_revisit
# speaker:Quin
Still stuck. Still losing time.
-> END

= quin_roadblock_after
{ roadApproach == "listen_first":
    # speaker:Quin
    That was not as ugly as I expected.
- else:
    # speaker:Quin
    It works.
}
-> END
