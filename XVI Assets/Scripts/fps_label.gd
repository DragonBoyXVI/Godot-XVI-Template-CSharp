@tool
extends Label;
class_name FpsLabel;
## A simple label node that displays the current game fps.
##
## Shows the game fps as text on a label, respects the same rules a 
## normal label does.


const TEXT_FORMAT := "FPS: %s";


## If true, the label only updates on physics frames.
@export var _on_physics: bool = false;


func _ready() -> void:
	
	if ( Engine.is_editor_hint() ):
		
		text = TEXT_FORMAT;
		set_process( false );
		set_physics_process( false );
		return;

func _validate_property( property: Dictionary ) -> void:
	if ( property[ "name" ] == "text" ):
		property[ "usage" ] = PROPERTY_USAGE_NONE;

func _process( _delta: float ) -> void:
	if ( not _on_physics ):
		text = TEXT_FORMAT % Engine.get_frames_per_second();

func _physics_process( _delta: float ) -> void:
	if ( _on_physics ):
		text = TEXT_FORMAT % Engine.get_frames_per_second();
