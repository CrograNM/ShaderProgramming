#version 330

layout(location=0) out vec4 FragColor;
in vec2 v_TPos;

void main()
{
	vec3 color;

	if (v_TPos.x + v_TPos.y < 0.5) { 
		color = vec3(1.0, 0.0, 0.0); // Red
	} 
	else {
		color = vec3(0.0, 0.0, 1.0); // Blue
	}
	FragColor = vec4(color, 1.0);
}
