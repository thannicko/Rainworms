class_name TurnStateMachine extends StateMachine

const MaxThrows: int = 8
const Worm: int = 6

signal points_earned(new_points: int)

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

var _is_invalid_throw: bool = false
var _invalid_throw_type: InvalidThrowType
var _nr_dices_left: int = MaxThrows
var _dices_frequency: Dictionary = {}
var _kept_dices: Dictionary = {}
var _player: Player

func set_points_earned(points: int) -> void:
	_player.points_earned_in_turn = points
	points_earned.emit(_player.points_earned_in_turn)

func get_points_earned() -> int:
	return _player.points_earned_in_turn

func buy_tile(tile: WormTile) -> void:
	set_points_earned(0)
	_player.tiles_bought.append(tile)

func invalidate_throw(reason: InvalidThrowType) -> void:
	_is_invalid_throw = true
	_invalid_throw_type = reason

func is_valid_throw() -> bool:
	return not _is_invalid_throw

func _has_no_worms() -> bool:
	return Worm not in _kept_dices.keys()

func invalid_throw_reason() -> String:
	if (_invalid_throw_type == InvalidThrowType.NO_DICES):
		return "Cannot keep any dices"
	elif (_invalid_throw_type == InvalidThrowType.NO_WORM):
		return "No worm thrown"
	elif (_invalid_throw_type == InvalidThrowType.NO_TILES):
		return "Cannot buy any tiles"
	return ""
