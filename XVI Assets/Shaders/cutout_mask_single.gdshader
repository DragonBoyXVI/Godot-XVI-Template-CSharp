shader_type canvas_item;
// checks the "r" value of the cutout text and blends a color
// into the base texture.
// If you use a grayscale texture, a white pixel is a full replacement, and
// a black one is no replacement.

uniform sampler2D cutout_texture: filter_nearest;
uniform vec3 replacement_color: source_color;

varying vec4 modulate;


void vertex() {
	modulate = COLOR;
}

void fragment() {
	// gets the modulate color
	vec4 base_color = texture( TEXTURE, UV );

	float cutout_value = texture( cutout_texture, UV ).r;
	vec3 mixed_color = mix( base_color.rgb, replacement_color, cutout_value );
	COLOR = vec4( mixed_color, base_color.a ) * modulate;
}
