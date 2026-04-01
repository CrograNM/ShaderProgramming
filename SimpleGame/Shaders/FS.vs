#version 330

in vec3 a_Pos;
in vec2 a_TPos;
out vec2 v_TPos;

uniform float u_Time;

void main()
{
	vec4 newPosition;
	newPosition = vec4(a_Pos, 1.0);
	gl_Position = newPosition;

    // v_TPos = a_TPos;
    
    vec2 uv;
    uv.x = a_Pos.x * 0.5 + 0.5;
    uv.y = -a_Pos.y * 0.5 + 0.5;
	v_TPos = uv;
}
