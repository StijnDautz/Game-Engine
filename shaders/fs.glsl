#version 330
 
// input
in vec2 uv;							// interpolated texture coordinates
in vec3 toCamera;					// intersection to camera
in vec3[4] toLight;					// intersection to light
uniform vec3 color;					// material color
uniform sampler2D pixels;			// texture sampler
uniform sampler2D normalMap;		// normal map
uniform int lightcount;				// total amount of lights to loop through
uniform float[4] Alightintensity;	// light intensities
uniform float[12] Alightcol;		// light colors

// output
out vec4 outputColor;				// color of the pixel

// fragment shader
void main()
{
	vec4 normal = normalize(vec4(texture2D(normalMap, uv) * 2f - 1));
	vec4 diffusecolor;
	// compute color a light cast on the pixel
	int c = 0;
	for(int i = 0; i < lightcount * 3; i+=3, c++)
	{
		// reconstruct vector3 from float[3]
		vec4 lightcol = vec4(Alightcol[i], Alightcol[i+1], Alightcol[i+2], 1);

		// phong shading
		// diffuse
		/// vertexWorldPos to lightPos
		vec3 L = toLight[c];
		float dist = length(L);
		L = normalize(L);
		diffusecolor = texture2D(pixels, uv) * lightcol;
		vec4 diffuse = diffusecolor * max(0.0f, dot(L, normal.xyz));
				
		// ambient
		vec4 ambient = diffusecolor * 0.005f;

		// specular
		vec4 specular = lightcol * pow(max(0.0f, dot(reflect(-L, normal.xyz), toCamera)), 30f);

		// output
		/// compute the light attenuation
		float attenuation = 1f / (0.1f * dist + .3f * dist * dist);
		///
		outputColor += Alightintensity[c] * (ambient + attenuation *  (diffuse + specular));
	}
	outputColor /= lightcount;
}