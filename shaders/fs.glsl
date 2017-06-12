#version 330
 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal
in vec3 worldPos;
uniform sampler2D pixels;		// texture sampler
uniform vec3 color;
uniform int lightcount;
uniform float[12] lights;
out vec4 outputColor;

// fragment shader
void main()
{
		int c = min(4, lightcount) * 3;
		for(int i = 0; i < c; i+=3)
		{
				// cast float[3] to vec3 
				vec3 lightpos = vec3(lights[i], lights[i+1], lights[i+2]);
				// vertexWorldPos to lightPos
				vec3 L = lightpos - worldPos;
				float distance = L.length();
				L = normalize(L);
				float attenuation = 1f / (distance * distance);

				outputColor += vec4(color + texture( pixels, uv ).xyz * (max(0.0f, dot(normal.xyz, L))), 1);
		}
		outputColor /= lightcount;
}