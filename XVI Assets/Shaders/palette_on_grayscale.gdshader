shader_type canvas_item;
// uses a palette texture to color a grayscale image.
// This only checks the "r" value of the base texture.
// Tip for creating palette textures via gradients, set the snap to be one
// color more and never assign a color to the end position.
// NOTE: for unknown reasons, the "modulate" properties only work right
// if the base texture is grayscale

uniform sampler2D palette_texture: filter_nearest;

varying vec4 modulate;

void vertex(){
	modulate = COLOR;
}

void fragment() {
	// get modulate color
	vec4 base_color = texture( TEXTURE, UV );
	
	vec4 palette_color = texture( palette_texture, vec2( base_color.r, 0.5  ) );
	COLOR = vec4( palette_color.rgb, base_color.a ) * modulate;
}
