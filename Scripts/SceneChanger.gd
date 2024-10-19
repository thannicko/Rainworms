extends Node

var _current_scene: Node

func _ready() -> void:
	_current_scene = get_tree().current_scene

func change_to_scene(scene: PackedScene, data := {}) -> void:
	print("SceneChanger ::  Changed to '", scene.resource_path, "' with data ", data)
	_current_scene.free()
	var new_scene := scene.instantiate()
	if new_scene.has_method("initialize"):
		new_scene.initialize(data)
	get_tree().root.add_child(new_scene)
	_current_scene = new_scene
