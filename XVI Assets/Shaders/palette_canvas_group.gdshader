shader_type canvas_item;
render_mode unshaded;
// mostly an experiment

uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;
uniform sampler2D palette_texture: filter_nearest;

varying vec4 modulate;

void vertex(){
	modulate = COLOR;
}

void fragment() {
	vec4 c = textureLod(screen_texture, SCREEN_UV, 0.0);
	
	if (c.a > 0.0001) {
		c.rgb /= c.a;
	}
	
	vec4 palette_color = texture( palette_texture, vec2( c.r, 0.5 ) );
	
	//COLOR *= c * palette_color;
	COLOR = vec4( palette_color.rgb, c.a ) * modulate;
}