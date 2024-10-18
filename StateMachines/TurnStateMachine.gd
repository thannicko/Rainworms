class_name TurnStateMachine extends StateMachine

const MaxThrows: int = 8
var _nr_dices_left: int = MaxThrows
var _dices_frequency: Dictionary = {}
var _kept_dices: Dictionary = {}


const _dice_textures: Dictionary = {
	1: "res://Assets/dice-one.png",
	2: "res://Assets/dice-two.png",
	3: "res://Assets/dice-three.png",
	4: "res://Assets/dice-four.png",
	5: "res://Assets/dice-five.png",
	6: "res://Assets/dice-six.png"
}
