#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED


void CalculateMainLight_float(float3 _worldPos, out float3 _direction, out float3 _color)
{
#if defined(SHADERGRAPH_PREVIEW)
	_direction = float3(0.5, 0.5, 0.0);
	_color = 1;
#else
	Light mainLight = GetMainLight();
	_direction = mainLight.direction;
	_color = mainLight.color;
#endif
}
#endif