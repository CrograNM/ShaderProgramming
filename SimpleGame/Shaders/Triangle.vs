#version 330

uniform float u_Time;

in vec3 a_Position;
in float a_Mass;
in vec2 a_Vel;
in float a_RV;

const float c_PI = 3.141592;
const vec2 c_G = vec2(0, -9.8);

void Basic()
{
	float t = mod(u_Time * 10, 1.0);
	vec4 newPosition;
	newPosition.x = a_Position.x + t;
	newPosition.y = a_Position.y;
	newPosition.z = a_Position.z;
	gl_Position = newPosition;
}

void Sin1()
{
	float t = mod(u_Time * 10, 1.0);
	vec4 newPosition;
	newPosition.x = a_Position.x + t;
	newPosition.y = a_Position.y + 0.5 * sin(t*2*3.141592);
	newPosition.z = a_Position.z;
	gl_Position = newPosition;
}

void Sin2()
{
	float t = mod(u_Time * 10, 1.0); // 0~1
	vec4 newPosition;
	newPosition.x = a_Position.x - 1 + t*2;
	newPosition.y = a_Position.y + 0.5 * sin(t*2*3.141592);
	newPosition.z = a_Position.z;
	gl_Position = newPosition;
}

void Circle()
{
	float t = mod(u_Time * 10, 1.0);
	float r = 0.5;
	vec4 newPosition;
	newPosition.x = a_Position.x + r * sin(t*2*3.141592);
	newPosition.y = a_Position.y + r * cos(t*2*3.141592);
	newPosition.z = a_Position.z;
	gl_Position = newPosition;
}

void Star()
{
    float speed = 10.0;
    float t = u_Time * speed;
    
    float r = 0.5 + 0.2 * cos(t * 5.0); 

    vec4 newPosition;

    float theta = t + 3.141592;
    newPosition.x = a_Position.x + r * cos(theta);
    newPosition.y = a_Position.y + r * sin(theta);
    
    newPosition.z = a_Position.z;
    newPosition.w = 1.0;

    gl_Position = newPosition;
}

void Lissajous()
{
    float t = u_Time * 10.0;
    
    float x = 0.7 * sin(3.0 * t);
    float y = 0.7 * sin(2.0 * t);

    vec4 newPosition;
    newPosition.x = a_Position.x + x;
    newPosition.y = a_Position.y + y;
    newPosition.z = a_Position.z;
    newPosition.w = 1.0;

    gl_Position = newPosition;
}

void Rose()
{
    float t = u_Time * 10.0;
    float k = 4.0;
    float r = 0.7 * cos(k * t);

    vec4 newPosition;
    newPosition.x = a_Position.x + r * cos(t);
    newPosition.y = a_Position.y + r * sin(t);
    newPosition.z = a_Position.z;
    newPosition.w = 1.0;

    gl_Position = newPosition;
}

void Falling()
{
    float t = mod(u_Time, 1.0);
    float tt = t*t;
    float vx = a_Vel.x;
    float vy = a_Vel.y;
    // 초기 위치를 원의 둘레 상으로만
    float initPosX = ( a_Position.x + sin(a_RV * 2 * c_PI) );
    float initPosY = ( a_Position.y + cos(a_RV * 2 * c_PI) );
    vec4 newPos;
    newPos.x = initPosX + vx * t + 0.5 * c_G.x * tt;
    newPos.y = initPosY + vy * t + 0.5 * c_G.y * tt;
    newPos.z = 0;
    newPos.w = 1;

    gl_Position = newPos;
}

void main()
{
	// Basic();
	// Sin1();
	// Sin2();
	// Circle();
	// Star();
	// Lissajous();
    // Rose();
    Falling();
}
