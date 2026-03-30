#version 330

layout(location=0) out vec4 FragColor;
in vec2 v_TPos;

void main()
{
	float R = v_TPos.x;
	float G = v_TPos.y;
	float B = 0.0;
	float A = 1.0;
	FragColor = vec4(R, G, B, A);
}
