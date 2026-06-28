shader_type canvas_item;

uniform vec3 target_color: source_color = vec3( 1.0 );

varying vec4 modulate;


void vertex() {
	modulate = COLOR;
}

void fragment() {
	vec4 tex_color = texture( TEXTURE, UV );
	
	const float third = 1.0/3.0;
	vec3 brightness = vec3( tex_color.r + tex_color.g + tex_color.b ) * third;
	
	COLOR = vec4( target_color * brightness, tex_color.a ) * modulate;
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
