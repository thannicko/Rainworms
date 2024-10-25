class_name WaitRethrowState extends State

@export var prompt_label: Label
@export var throw_dice_button: Button
@export var throw_dice_container: Container
@export var deck_controller: DeckController

func enter(data := {}) -> void:
	prompt_label.text = "Throw the dice or click on a tile to buy"
	_clear_container(throw_dice_container)
	
	throw_dice_button.disabled = false
	throw_dice_button.button_down.connect(_on_throw_dice_button_down)
	throw_dice_button.show()
	
	deck_controller.set_enable_buying(true)
	deck_controller.tile_bought.connect(_on_tile_bought)

func _on_tile_bought(tile: WormTile) -> void:
	(self._state_machine as TurnStateMachine).buy_tile(tile)
	prompt_label.text = "Successfully bought: " + tile.display_details()
	await get_tree().create_timer(1.0).timeout
	self._state_machine._change_to_state("InitialThrowState")

func _on_throw_dice_button_down():
	self._state_machine._change_to_state("ThrowingState")

func _on_stop_throw_button_down():
	self._state_machine._change_to_state("StopThrowState")

func _clear_container(container: Container) -> void:
	for child in container.get_children():
		child.queue_free()

func exit() -> void:
	deck_controller.set_enable_buying(false)
	throw_dice_button.button_down.disconnect(_on_throw_dice_button_down)
