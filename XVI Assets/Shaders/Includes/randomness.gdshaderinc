
float random( vec2 st ) {
	const vec2 multv2 = vec2(12.9898, 78.233);
	const float multf = 43758.5453123;
	
	return fract( sin( dot( st, multv2 ) ) * multf );
}