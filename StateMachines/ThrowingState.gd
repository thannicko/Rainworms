class_name ThrowingState extends State

@export var throw_dice_button: Button
@export var stop_throw_button: Button
@export var throw_dice_container: Container
@export var dice_animation: AnimatedSprite2D
@export var prompt_label: Label

var _dice_textures_to_value: Dictionary = {}

func enter(data := {}) -> void:
	_dice_textures_to_value.clear()
	(self._state_machine as TurnStateMachine)._dices_frequency.clear()
	throw_dice_button.disabled = true
	stop_throw_button.disabled = true
	prompt_label.hide()
	_start_throw()

func _start_throw():
	dice_animation.play()
	dice_animation.show()
	var timer := get_tree().create_timer(1.0)
	timer.timeout.connect(_end_throw)
	
func _end_throw():
	var state_machine := (self._state_machine as TurnStateMachine)
	dice_animation.hide()
	var rng = RandomNumberGenerator.new()
	for i in range(0, state_machine._nr_dices_left):
		var dice = rng.randi_range(1, 6)
		_store_dice_frequency(dice)
		_spawn_dice_texture(dice)
	var allowed_dices: Array = state_machine._dices_frequency.keys().filter(
		func(dice): return _is_allowed_to_keep(dice))
	if allowed_dices.is_empty():
		state_machine._change_to_state("StopThrowState", { 'invalid_thrown': true })
		prompt_label.text = "Invalid throw!"
	else:
		prompt_label.text = "Click on a dice to keep it"
	prompt_label.show()

func _store_dice_frequency(dice: int) -> void:
	var dices_frequency: Dictionary = (self._state_machine as TurnStateMachine)._dices_frequency
	if dice in dices_frequency.keys():
		dices_frequency[dice] = dices_frequency[dice] + 1
	else:
		dices_frequency[dice] = 1

func _spawn_dice_texture(dice: int) -> void:
	var new_dice := TextureRect.new()
	new_dice.texture = load((self._state_machine as TurnStateMachine)._dice_textures[dice])
	new_dice.expand_mode = TextureRect.EXPAND_FIT_WIDTH_PROPORTIONAL
	if _is_allowed_to_keep(dice):
		new_dice.gui_input.connect(_on_gui_input_dice.bind(dice))
	else:
		new_dice.modulate = Color.GRAY
	throw_dice_container.add_child(new_dice)
	_dice_textures_to_value[new_dice] = dice

func _on_gui_input_dice(event: InputEvent, dice: int) -> void:
	if (event.is_pressed() and event is InputEventMouseButton):
		self._state_machine._change_to_state("KeepDiceState", { 'dice_value' : dice, 'dice_textures' : _dice_textures_to_value })

func _is_allowed_to_keep(dice: int) -> bool:
	return dice not in (self._state_machine as TurnStateMachine)._kept_dices.keys()

func exit() -> void:
	for dice_texture in throw_dice_container.get_children():
		(dice_texture as TextureRect).gui_input.disconnect(_on_gui_input_dice)
