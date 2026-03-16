#version 330

uniform float u_Time;

in vec3 a_Position;

void Sin1() {
	float t = u_Time * 1;

	vec4 newPosition;
	newPosition.x = a_Position.x + t;

	// A * sin(B * t), A = 0.5 (최대 진폭), B = 2 * pi (2pi = 1주기)
	newPosition.y = a_Position.y + 0.5 * sin(2.0 * 3.141592 * t); 

	newPosition.z = 0;
	newPosition.w = 1;
	
	gl_Position = newPosition;
}

void Sin2() {
	float t = mod(u_Time, 1.0f) * 2 - 1;

	vec4 newPosition;
	newPosition.x = a_Position.x + t;
	newPosition.y = a_Position.y - 0.5 * sin(3.141592 * t); 
	newPosition.z = 0;
	newPosition.w = 1;

	gl_Position = newPosition;
}

void Circle1() {
	float t = u_Time * 1;
	vec4 newPosition;
	newPosition.x = a_Position.x + cos(2.0 * 3.141592 * t);		// 1주기, 반지름 1
	newPosition.y = a_Position.y + sin(2.0 * 3.141592 * t);		// 1주기, 반지름 1
	newPosition.z = 0;
	newPosition.w = 1;
	gl_Position = newPosition;
}

void main()
{
	Circle1();
}
