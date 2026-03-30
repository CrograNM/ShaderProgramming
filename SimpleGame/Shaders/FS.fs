#version 330

in vec2 vUV;
layout(location=0) out vec4 FragColor;

void main()
{
	float R = vUV.x;
	float G = vUV.y;
	float B = 0.0;
	float A = 1.0;
	FragColor = vec4(R, G, B, A);
}
