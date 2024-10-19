class_name TurnStateMachine extends StateMachine

const MaxThrows: int = 8
const Worm: int = 6

var _nr_dices_left: int = MaxThrows
var _dices_frequency: Dictionary = {}
var _kept_dices: Dictionary = {}
var _current_points: int = 0
var _player: Player

const _dice_textures: Dictionary = {
	1: "res://Assets/dice-one.png",
	2: "res://Assets/dice-two.png",
	3: "res://Assets/dice-three.png",
	4: "res://Assets/dice-four.png",
	5: "res://Assets/dice-five.png",
	6: "res://Assets/worm.png"
}

const _dice_side_to_points: Dictionary = {
	1 : 1,
	2 : 2,
	3 : 3,
	4 : 4,
	5 : 5,
	6 : 5, # This is the worm
}

enum InvalidThrowType { NO_DICES, NO_WORM, NO_TILES }

func _has_no_worms() -> bool:
	return Worm not in _kept_dices.keys()

func _invalid_throw_type_to_string(type: InvalidThrowType) -> String:
	if (type == InvalidThrowType.NO_DICES):
		return "Cannot keep any dices"
	elif (type == InvalidThrowType.NO_WORM):
		return "No worm thrown"
	elif (type == InvalidThrowType.NO_TILES):
		return "Cannot buy any tiles"
	return ""
