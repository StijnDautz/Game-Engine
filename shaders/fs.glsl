#version 330
 
// shader input
in vec2 uv;						// interpolated texture coordinates
in vec4 normal;					// interpolated normal
in vec3 worldPos;
uniform sampler2D pixels;		// texture sampler
uniform vec3 color;
uniform vec3 camerapos;
uniform int lightcount;
uniform float[12] Alightpos;
uniform float[12] Alightcol;
uniform float[4] Alightintensity;

out vec4 outputColor;

// fragment shader
void main()
{
		int c = 0;
		for(int i = 0; i < lightcount * 3; i+=3, c++)
		{
				// reconstruct vector3 from float[3]
				vec3 lightpos = vec3(Alightpos[i], Alightpos[i+1], Alightpos[i+2]);
				vec3 lightcol = vec3(Alightcol[i], Alightcol[i+1], Alightcol[i+2]);
				
				// diffuse
				/// vertexWorldPos to lightPos
				vec3 L = lightpos - worldPos;
				float dist = length(L);
				L = normalize(L);
				vec3 diffusecolor = color.xyz + texture(pixels, uv).xyz * lightcol;
				vec3 diffuse = diffusecolor * (max(0.0f, dot(L, normal.xyz)));
				
				// ambient
				vec3 ambient = diffusecolor * 0.12f;

				// specular
				vec3 specular = diffusecolor * pow(max(0.0f, dot(reflect(-L, normal.xyz), normalize(camerapos - worldPos))), 40f);

				// output
				float attenuation = 1f / (1 + 0.7f * dist * dist);
				outputColor += vec4(ambient, 1) + attenuation * Alightintensity[c] * vec4(diffuse + specular, 1);
		}
		outputColor /= lightcount;
}