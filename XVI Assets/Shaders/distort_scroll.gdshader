shader_type canvas_item;

uniform float distort_strength: hint_range(0.0, 1.0, 0.01) = 0.05;
uniform vec2 distort_scroll = vec2( 0.125 );
uniform ivec2 distort_tile = ivec2( 1 );
uniform float distort_rotate_speed = 0.0;
uniform sampler2D distortion_map;
uniform vec2 base_scroll = vec2( 0.05 );
uniform float base_rotate_speed = 0.0;
uniform ivec2 base_tile = ivec2( 1 );

varying vec4 modulate;

void vertex() {
	modulate = COLOR;
}

void fragment() {
	// bear with me here qwqq
	float _distort_rotate_time = TIME * distort_rotate_speed;
	mat2 distort_matrix = mat2( 
		vec2( cos( _distort_rotate_time ), -sin( _distort_rotate_time ) ),
		vec2( sin( _distort_rotate_time ), cos( _distort_rotate_time ) )
	 );
	
	vec2 distort_sample_uv = fract( ( UV + ( distort_scroll * TIME ) ) );
	distort_sample_uv -= 0.5;
	distort_sample_uv *= distort_matrix;
	distort_sample_uv = fract( distort_sample_uv + 0.5 );
	distort_sample_uv = fract( distort_sample_uv * vec2( distort_tile ) );
	
	float _base_rotate_time = TIME * base_rotate_speed;
	mat2 base_matrix = mat2( 
		vec2( cos( _base_rotate_time ), -sin( _base_rotate_time ) ),
		vec2( sin( _base_rotate_time ), cos( _base_rotate_time ) )
	 );
	
	vec2 base_uv = fract( UV + ( TIME * base_scroll ) );
	base_uv -= 0.5;
	base_uv *= base_matrix;
	base_uv = fract( base_uv + 0.5 );
	base_uv = fract( base_uv * vec2( base_tile ) );
	
	vec4 distort_sample = texture( distortion_map, distort_sample_uv );
	vec2 distort_uv = ( distort_sample.xy - 0.5 ) * 2.0;
	distort_uv *= distort_strength;
	vec2 final_uv = fract( base_uv + distort_uv );
	
	COLOR = texture( TEXTURE, final_uv ) * modulate;
}
