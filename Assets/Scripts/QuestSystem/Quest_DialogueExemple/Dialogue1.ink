=== Dialogue1Start ===

{ Dialogue1State :
    - "REQUIREMENTS_NOT_MET": -> requirementsNotMet
    - "CAN_START": -> canStart
    - "FINISHED": -> finished
    - else: -> END
}

VAR cohesionLevel = 10

= requirementsNotMet
// not possible for this quest, but putting something here anyways
Come back once you're on good terms with friends.
-> END

= canStart
(Joseph) So, how's the project?  CLevel: {cohesionLevel}

* {cohesionLevel > 5} [Not so bad at all! What about you? (5+ Cohesion)]
    ~ CohesionLevelGained(5)
    ~ Dialogue1State = "FINISHED"
    -> CohesionContinue

* [I'm doing just fine.]
    Ah, alright...
    -> END

* [It's none of your business pipsqueak.]
    ~ CohesionLevelGained(-5)
    ~ Dialogue1State = "FINISHED"
    Man f-
    -> END

= CohesionContinue
Oh! Well, I've done some amount of work!

* [Let's get it]
    -> END

= finished
{cohesionLevel < 5:
    I'm not in the mood to talk.
- else:
    -> Cooking
    
}
-> END

= Cooking
Shall we get to cooking?
    * [Aye]
    -> END

