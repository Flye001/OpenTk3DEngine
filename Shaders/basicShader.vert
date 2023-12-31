#version 330 core
layout (location = 0) in vec3 inPos;
layout (location = 1) in vec3 inNormal;
layout (location = 2) in vec3 inColor;

out vec4 vertexColor;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    gl_Position = vec4(inPos, 1.0) * model * view * projection;

    // Light Direction
    vec4 lightPosition = vec4(0.0, 1.0, 1.0, 1.0);// * model * view;
    vec3 lightDir = normalize(lightPosition.xyz);
    float shade = max(0.1, dot(lightDir, inNormal));

    vertexColor = vec4(inColor * shade, 1.0);
    //vertexColor = vec4(inColor, 1.0);
}