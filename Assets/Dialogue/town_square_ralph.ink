=== ralph_dialogue ===
{ SpiretonMainQuestState:
    - "REQUIREMENTS_NOT_MET":   -> ralph_before
    - "CAN_START":              -> ralph_before
    - "IN_PROGRESS":            -> ralph_during
    - "CAN_FINISH":             -> ralph_during
    - "FINISHED":               -> ralph_after
    - else:                     -> END
}

= ralph_before
# speaker:Ralph
Oh good. Someone to talk to. I've been standing here trying to look purposeful.

+ [Is it working?]
    -> ralph_purposeful
+ [Are you nervous?]
    -> ralph_nervous

= ralph_purposeful
# speaker:Ralph
Probably not. Joyce keeps glancing over like she's already disappointed in me.

# speaker:Ralph
Which, to be fair, is a reasonable baseline.
-> END

= ralph_nervous
# speaker:Ralph
A bit, yeah. I do better when I know what the job is.

# speaker:Ralph
Right now the job is just standing here, and somehow I'm still managing to do it wrong.
-> END

= ralph_during
# speaker:Ralph
Still here. Still helpful. Mostly.

+ [Staying focused?]
    -> ralph_focused
+ [Any questions about the plan?]
    -> ralph_plan

= ralph_focused
# speaker:Ralph
Trying. My brain keeps offering me alternative ideas that nobody asked for.

# speaker:Ralph
I've been saying no to them. It's going okay.
-> END

= ralph_plan
# speaker:Ralph
A few, but they're the kind I can figure out by just watching what everyone else does.

# speaker:Ralph
I find that works better than asking and then immediately forgetting the answer.
-> END

= ralph_after
# speaker:Ralph
We made it. All of us. Including me, which I'll admit was not guaranteed.

+ [You came through when it counted.]
    -> ralph_came_through
+ [What did you learn today?]
    -> ralph_learned

= ralph_came_through
# speaker:Ralph
Eventually. After a detour involving a rabbit.

# speaker:Ralph
I'm choosing to remember the coming through part more than the rabbit part.
-> END

= ralph_learned
# speaker:Ralph
Ask before you wander off. Stay where someone can see you.

# speaker:Ralph
And rabbits are not, in fact, ever leading anywhere useful.
-> END
