#version 330

in vec3 a_Pos;
out vec2 vUV;

void main()
{
	vec4 newPosition;
	newPosition = vec4(a_Pos, 1.0);
	gl_Position = newPosition;
	
	// 위치에 따라 각각 다른 out 값 주기 (-1 --> 0, 1 --> 1)
	vUV = a_Pos.xy * 0.5 + 0.5; 
}
