=== aisha_roadblock ===
{ roadBlockResolved:
    -> aisha_roadblock_after
}

# speaker:Aisha
Before anyone touches that cart, we should know what both of them actually need.

* [What did you notice about the two of them?]
    -> aisha_notice
* [Do you think we can satisfy both?]
    -> aisha_both
* [How do you want to handle this?]
    -> aisha_handle

= aisha_notice
# speaker:Aisha
Quin wants the cart straight. Liora wants the bank intact.

# speaker:Aisha
Those are different problems with the same solution, if you are careful. But nobody has asked the right question yet.

-> END

= aisha_both
# speaker:Aisha
Yes. If we name both needs out loud before we touch anything, we end up solving both at once.

# speaker:Aisha
The mistake would be to only fix the one we heard first.

-> END

= aisha_handle
# speaker:Aisha
Ask them each what they need. Not what they want from each other. What they actually need from us.

# speaker:Aisha
Then we have something to work with.

-> END

= aisha_roadblock_after
{ roadApproach == "listen_first":
    # speaker:Aisha
    Good. That is exactly why we said it.
- else:
    # speaker:Aisha
    That gets us through. It does not make it a good answer.
    { primaryNorm == "respect":
        # speaker:Aisha
        We said problem, not people.
    }
}
-> END
