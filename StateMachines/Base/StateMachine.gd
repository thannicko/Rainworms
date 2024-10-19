class_name  StateMachine extends Node

var _current_state: State

func _ready():
	for state in get_children():
		(state as State)._state_machine = self
	var initial_state := get_children()[0]
	_change_to_state(initial_state.name)

func handle_input(_event: InputEvent) -> void:
	_current_state.handle_input(_event)

func update(_delta: float) -> void:
	_current_state.update(_delta)

func physics_update(_delta: float) -> void:
	_current_state.physics_update(_delta)

func _change_to_state(state_name: String, data := {}) -> void:
	if OS.is_debug_build():
		print("StateMachine :: ", _current_state, " --> ", state_name, " with data: ", data)
	var state: State = find_child(state_name)
	if (_current_state != null):
		_current_state.exit()
	_current_state = state
	_current_state.enter(data)
