#version 330

uniform float u_time;

in vec3 a_Position;

void main()
{
	vec4 newPosition;

	newPosition.x = a_Position.x + 0.5;
	newPosition.y = a_Position.y;
	newPosition.z = 0;
	newPosition.w = 1;
	
	gl_Position = newPosition;
}
