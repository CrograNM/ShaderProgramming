#version 330

uniform float u_Time;

in vec3 a_Pos;
out float v_Grey;


const float c_PI = 3.141592;

void main()
{
    float time = u_Time * 0.01;
    float value = a_Pos.x + 0.5;
    
    float newX = a_Pos.x;
    float newY = a_Pos.y + 
        value * 0.3 * sin((newX+0.5) * 2 * c_PI - time);
    
    vec4 final = vec4(newX, newY, 0.0, 1.0);
    
	vec4 newPosition = final;
	
	float grey = (sin((newX+0.5) * 2 * c_PI - time) + 1.0) * 0.5;
	v_Grey = grey;
	
	gl_Position = newPosition;
}
