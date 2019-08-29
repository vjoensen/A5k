#version 450 core
out vec4 FragColor;
  

in vec2 TexCoord;

uniform sampler2D ourTexture;
uniform vec3 spriteColor;

void main()
{
    FragColor = vec4(spriteColor,1.0) * texture(ourTexture, TexCoord);
} 