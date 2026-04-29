=== liora_roadblock ===
{ roadBlockResolved:
    -> liora_roadblock_after
}
{ spokeTo_Liora:
    -> liora_roadblock_revisit
}

# speaker:Liora
Nobody is listening. This is exactly how banks give out.

* [What is your concern with the bank?]
    -> liora_bank
* [What would you do to free the cart safely?]
    -> liora_safe
* [We hear you. Tell us what cannot happen.]
    -> liora_warning

= liora_bank
# speaker:Liora
The road edge here is loose stone over old fill. Too much weight in the wrong place and the whole side drops.

# speaker:Liora
Once that goes, there is no road for anyone for weeks.

# speaker:You
That matters. We will keep the weight where the ground is solid.

~ spokeTo_Liora = true
-> END

= liora_safe
# speaker:Liora
Lift from the front, not the side. Keep the stones in place. Let the wheel find the rut, do not force it.

# speaker:Liora
And keep everyone off the soft edge while you do it.

# speaker:You
Good. That is something we can work with.

~ spokeTo_Liora = true
-> END

= liora_warning
# speaker:Liora
Do not touch the bank stones. Do not let anyone stand on the soft edge. And do not let Quin rush it.

# speaker:You
Noted. All of it.

~ spokeTo_Liora = true
-> END

= liora_roadblock_revisit
# speaker:Liora
The bank is still too rough for my liking. Be careful.
-> END

= liora_roadblock_after
{ roadApproach == "listen_first":
    # speaker:Liora
    Thank you for not making this uglier.
- else:
    # speaker:Liora
    You cleared the cart, but the bank is rougher than I like.

    # speaker:Liora
    It will hold for now. I am coming back with tools tomorrow.
}
-> END
