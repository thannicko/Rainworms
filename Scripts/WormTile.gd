class_name WormTile extends Resource

@export var cost: int = 0
@export var worm: int = 0

func buy_details() -> String:
	return str(cost) + "\n" + _worm_string()

func display_details() -> String:
	return "Tile " + str(cost) + " for " + _worm_string()

func _worm_string() -> String:
	var worm_string = str(worm) + " worm"
	if (worm > 1):
		worm_string += "s"
	return worm_string
