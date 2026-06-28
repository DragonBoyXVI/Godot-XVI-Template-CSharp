shader_type canvas_item;

uniform sampler2D screen: hint_screen_texture;

uniform vec3 tint_color: source_color = vec3( 1.0 );
uniform float tint_strength: hint_range(0.0, 1.0, 0.01) = 1.0;

void fragment() {
	vec4 texture_color = texture( TEXTURE, UV );
	vec4 base_color = texture( screen, SCREEN_UV );
	const float div = 1.0/3.0;
	float black_strength = ( base_color.r + base_color.g + base_color.b ) * div;
	float white_strength = 1.0 - black_strength;
	float base_strength = min( black_strength, white_strength ) * texture_color.r;
	
	float strength = tint_strength * base_strength;
	COLOR = vec4( mix( base_color.rgb, tint_color, strength ), base_color.a );
}
