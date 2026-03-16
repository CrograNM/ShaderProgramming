#version 330

const float PI = 3.14159265358;

uniform float u_Time;

in vec3 a_Position;

void Sin1() {
	float t = u_Time * 1;

	vec4 newPosition;
	newPosition.x = a_Position.x + t;

	// A * sin(B * t), A = 0.5 (최대 진폭), B = 2 * pi (2pi = 1주기)
	newPosition.y = a_Position.y + 0.5 * sin(2.0 * PI * t); 

	newPosition.z = 0;
	newPosition.w = 1;
	
	gl_Position = newPosition;
}

void Sin2() {
	float t = mod(u_Time, 1.0f) * 2 - 1;

	vec4 newPosition;
	newPosition.x = a_Position.x + t;
	newPosition.y = a_Position.y - 0.5 * sin(PI * t); 
	newPosition.z = 0;
	newPosition.w = 1;

	gl_Position = newPosition;
}

void Circle1() {
	float t = u_Time * 1;
	vec4 newPosition;
	newPosition.x = a_Position.x + cos(2.0 * PI * t);		// 1주기, 반지름 1
	newPosition.y = a_Position.y + sin(2.0 * PI * t);		// 1주기, 반지름 1
	newPosition.z = 0;
	newPosition.w = 1;
	gl_Position = newPosition;
}

void StarMove() { 
    // flower curve
    float t = u_Time * 2.0 * PI;
    
    // r = a + b * cos(k * t) 형태의 극좌표계를 이용
    // k가 5이면 5개의 꼭짓점을 가집니다.
    float r = 0.5 + 0.3 * cos(5.0 * t);
    
    vec4 newPosition;
    newPosition.x = a_Position.x + r * cos(t);
    newPosition.y = a_Position.y + r * sin(t);
    newPosition.z = 0.0;
    newPosition.w = 1.0;
    
    gl_Position = newPosition;
}

void InfinityMove() {
    float t = u_Time * 2.0 * PI;
    
    // 8자 모양 수식의 전형적인 매개변수 방정식
    float scale = 0.8;
    float denom = 1.0 + pow(sin(t), 2.0);
    
    vec4 newPosition;
    newPosition.x = a_Position.x + (scale * cos(t)) / denom;
    newPosition.y = a_Position.y + (scale * sin(t) * cos(t)) / denom;
    newPosition.z = 0.0;
    newPosition.w = 1.0;
    
    gl_Position = newPosition;
}

void main()
{
	StarMove();
}
