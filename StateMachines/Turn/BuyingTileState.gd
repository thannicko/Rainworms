class_name BuyingTileState extends State

@export var prompt_label: Label
@export var deck_controller: DeckController

func enter(data := {}) -> void:
	prompt_label.text = "Click on a tile to buy"
	deck_controller.tile_bought.connect(_on_tile_bought)
	deck_controller.enable_buying()

func _on_tile_bought(tile: WormTile) -> void:
	(self._state_machine as TurnStateMachine).buy_tile(tile)

func exit() -> void:
	deck_controller.tile_bought.disconnect(_on_tile_bought)
