#version 330

in vec3 a_Pos;

void main()
{
	vec4 newPosition;
	newPosition = vec4(a_Pos, 1.0);
	gl_Position = newPosition;
}
