// external functions
EXTERNAL StartQuest(questId)
EXTERNAL AdvanceQuest(questId)
EXTERNAL FinishQuest(questId)
EXTERNAL SwitchScene(sceneName)
EXTERNAL DestroyNPC()

// ink fallbacks used by InkPlayerWindow when C# bindings are absent
=== function StartQuest(questId) ===
~ return
=== function AdvanceQuest(questId) ===
~ return
=== function FinishQuest(questId) ===
~ return
=== function SwitchScene(sceneName) ===
~ return
=== function DestroyNPC() ===
~ return

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

// road block scene variables
VAR roadApproach = ""
VAR trustState = ""
VAR roadBlockResolved = false
VAR spokeTo_Quin = false
VAR spokeTo_Liora = false
VAR examined_wheel = false
VAR examined_crate = false

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
INCLUDE road_block_scene.ink
INCLUDE road_block_quin.ink
INCLUDE road_block_liora.ink
INCLUDE road_block_joyce.ink
INCLUDE road_block_aisha.ink
