shader_type canvas_item;

uniform vec2 scroll_speed = vec2( 1.0 );

varying vec4 modulate;

void vertex() {
	modulate = COLOR;
}

void fragment() {
	vec2 new_uv = fract( UV + ( scroll_speed * TIME ) );
	COLOR = texture( TEXTURE, new_uv ) * modulate;
}

