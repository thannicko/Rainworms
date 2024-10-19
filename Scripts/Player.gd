class_name Player extends RefCounted

var name: String
var points_earned_in_turn: int
var tiles_bought: Array[WormTile] = []

func buy_tile(tile: WormTile) -> void:
	tiles_bought.append(tile)
	points_earned_in_turn = 0
