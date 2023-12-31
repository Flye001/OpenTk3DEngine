#version 330 core
layout (location = 0) in vec3 inPos;
layout (location = 1) in vec3 inColor;

out vec4 vertexColor;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    gl_Position = vec4(inPos, 1.0) * model * view * projection;
    vec3 temp = gl_Position.xyz;
    vec3 temp_n = normalize(temp);
    gl_Position = vec4(temp_n, 1.0);
    vertexColor = vec4(inColor, 1.0);
}