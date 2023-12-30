#version 330 core
layout (location = 0) in vec3 inPos;

out vec4 vertexColor;

uniform mat4 projMatrix;

void main()
{
    gl_Position = vec4(inPos, 1.0) * projMatrix;
    vec3 temp = gl_Position.xyz;
    vec3 temp_n = normalize(temp);
    gl_Position = vec4(temp_n, 1.0);
    vertexColor = vec4(1.0, 1.0, 0.0, 1.0);
}