shader_type canvas_item;
// identical to the single color one, but now we use the rbg channels
// for three diffrent colors.


uniform sampler2D cutout_texture: filter_nearest;
uniform vec3 replacement_color_r: source_color = vec3( 1.0, 0.0, 0.0 );
uniform vec3 replacement_color_g: source_color = vec3( 0.0, 1.0, 0.0 );
uniform vec3 replacement_color_b: source_color = vec3( 0.0, 0.0, 1.0 );

varying vec4 modulate;


void vertex() {
	modulate = COLOR;
}

void fragment() {
	// gets the modulate color
	vec4 base_color = texture( TEXTURE, UV );

	vec4 cutout_color = texture( cutout_texture, UV );
	vec3 mixed_color = mix( base_color.rgb, replacement_color_r, cutout_color.r );
	mixed_color = mix( mixed_color, replacement_color_g, cutout_color.g );
	mixed_color = mix( mixed_color, replacement_color_b, cutout_color.b );

	COLOR = vec4( mixed_color, base_color.a ) * modulate;
}
