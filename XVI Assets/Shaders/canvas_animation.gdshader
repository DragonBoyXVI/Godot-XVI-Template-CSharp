shader_type canvas_item;

uniform float speed = 1.0;
uniform float time_offset = 0.0;

group_uniforms Rotate;
uniform float rotate_speed = 0.0;
uniform float rotate_offset = 0.0;

group_uniforms Wave;
uniform vec2 wave_speed = vec2( 1.0 );
uniform float wave_offset = 0.0;
uniform vec2 wave_amplitude = vec2( 0.0 );
uniform float wave_offset_per_vertex = 5.0;

group_uniforms Pulse;
uniform vec2 pulse_speed = vec2( 1.0 );
uniform float pulse_offset = 0.0;
uniform vec2 pulse_amplitude = vec2( 0.0 );

group_uniforms PaletteColoring;
uniform bool palette_enabled = false;
uniform sampler2D palette_texture;

varying vec4 modulate;

void vertex() {
	modulate = COLOR;
	float time = ( TIME + time_offset ) * speed;
	
	//rotate
	float rotate_time = ( time + rotate_offset ) * rotate_speed;
	mat2 rotation_matrix = mat2(
		vec2( cos( rotate_time ), -sin( rotate_time ) ),
		vec2( sin( rotate_time ), cos( rotate_time ) ) );
	VERTEX.xy *= rotation_matrix;
	
	//wave
	float wave_time = ( ( time + wave_offset ) + ( float( VERTEX_ID ) * wave_offset_per_vertex ) );
	VERTEX.xy += vec2( sin( wave_time * wave_speed.x ), cos( wave_time * wave_speed.y ) ) * wave_amplitude;
	
	//pulse
	float pulse_time = ( time + pulse_offset );
	vec2 pulse_vector = vec2( 1.0 );
	pulse_vector.xy += vec2( sin( pulse_time * pulse_speed.x ), sin( pulse_time * pulse_speed.y ) ) * pulse_amplitude;
	VERTEX.xy *= pulse_vector;
}

void fragment(){
	if ( palette_enabled ) {
		vec4 base_color = texture( TEXTURE, UV );
		
		vec4 palette_color = texture( palette_texture, vec2( base_color.r, 0.5  ) );
		COLOR = vec4( palette_color.rgb, base_color.a ) * modulate;
	}
}