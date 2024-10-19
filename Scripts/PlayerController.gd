extends Node

var _players: Array[Player]
var _active_player: Player
var _deck: TileDeck = load("res://Deck.tres")

func _ready() -> void:
	var test_player := Player.new()
	test_player.name = "Chiichii"
	test_player.points_earned_in_turn = 30
	_players.append(test_player)
	_active_player = test_player
