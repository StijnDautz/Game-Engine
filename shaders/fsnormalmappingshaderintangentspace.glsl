#version 330
 
// input
in vec2 uv;							// interpolated texture coordinates
//in vec4 normal;						// interpolated normal
in vec3 tangent;					// tangent vector
in vec3 worldPos;					// vertex position
in vec3 cameraPosition;				// camera position
in vec3 bitangent;
in vec3[4] lightpositions;			// light positions
uniform sampler2D pixels;			// texture sampler
uniform sampler2D normalMap;		// normal map
uniform vec3 color;					// material color
uniform int lightcount;				// total amount of lights to loop through
uniform float[4] Alightintensity;	// light intensities
uniform float[12] Alightcol;		// light colors

// output
out vec4 outputColor;				// color of the pixel

// fragment shader
void main()
{
	vec3 normal = normalize(vec4(2.0 * texture(normalMap, uv) - 1).xyz);
	// compute color a light cast on the pixel
	int c = 0;
	for(int i = 0; i < lightcount * 3; i+=3, c++)
	{
			// reconstruct vector3 from float[3]
			vec4 lightcol = vec4(Alightcol[i], Alightcol[i+1], Alightcol[i+2], 1);

			// phong shading
			// diffuse
			/// vertexWorldPos to lightPos
			vec3 L = lightpositions[c] - worldPos;
			float dist = length(L);
			L = normalize(L);
			vec4 diffusecolor = vec4(color, 1) + texture(pixels, uv) * lightcol;
			vec4 diffuse = diffusecolor * max(0.0f, dot(L, normal.xyz));
				
			// ambient
			vec4 ambient = diffusecolor * 0.0052f;

			// specular
			vec4 specular = diffusecolor * pow(max(0.0f, dot(reflect(-L, normal.xyz), normalize(cameraPosition - worldPos))), 30f);

			// output
			/// compute the light attenuation
			float attenuation = 1f / (1 + 0.3f * dist * dist);
			///
			outputColor += Alightintensity[c] * (ambient + attenuation *  (diffuse + specular));
	}
	outputColor /= lightcount;
	//outputColor = vec4(bitangent, 0);
}