shader_type canvas_item;
// this only checks the "r" value of the disolve texture.

uniform float progress: hint_range( 0.0, 1.0, 0.01 ) = 0.0;
uniform sampler2D disolve_texture: filter_nearest;

void fragment() {
	vec4 disolve_color = texture( disolve_texture, UV );
	if ( disolve_color.r <= progress ) {
		COLOR = vec4( 0.0 );
	}
}
