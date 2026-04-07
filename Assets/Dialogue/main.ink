// external functions
EXTERNAL StartQuest(questId)
EXTERNAL AdvanceQuest(questId)
EXTERNAL FinishQuest(questId)
EXTERNAL SwitchScene(sceneName)

// quest ids (questId + "Id" for variable name)
VAR CollectCoinsQuestId = "CollectCoinsQuest"
VAR SpiretonMainQuestId = "SpiretonMainQuest"

// quest states (questId + "State" for variable name)
VAR CollectCoinsQuestState = "REQUIREMENTS_NOT_MET"
VAR SpiretonMainQuestState = "REQUIREMENTS_NOT_MET"

VAR npcSmallTalk_State = "REQUIREMENTS_NOT_MET"

// spriteton story variables
VAR primaryNorm = ""
VAR leadStyle = ""

// ink files
INCLUDE collect_coins_start_npc.ink
INCLUDE collect_coins_finish_npc.ink
INCLUDE npc_small_talk.ink
INCLUDE npc_random_dialogue.ink
INCLUDE npc_trpg.ink
INCLUDE town_square_mayor.ink
INCLUDE town_square_guard.ink
INCLUDE town_square_joyce.ink
INCLUDE town_square_aisha.ink
INCLUDE town_square_ralph.ink
