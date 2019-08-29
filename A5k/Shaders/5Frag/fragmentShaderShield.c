#version 450 core
out vec4 FragColor;
  

in vec2 TexCoord;

uniform sampler2D ourTexture;
uniform vec4 spriteColor;

void main()
{
	vec4 mask = texture(ourTexture, TexCoord);
    FragColor = vec4(spriteColor.xyz,spriteColor.w*mask.a) ;
} 