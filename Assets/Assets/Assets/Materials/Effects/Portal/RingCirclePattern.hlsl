float2 center = float2(0.5, 0.5); 
float2 size = float2(0.5, 0.5); 
float2 result = float2(1.0, 1.0);
    

for (int i = 0; i < 10; i++)
{
    float angle = radians(i * (360.0 / 10.0)); 
        
    float2 ellipsePos = center + size * float2(cos(angle), sin(angle));
        
    float distance = length(uv - ellipsePos);
        
    result += smoothstep(0.005, 0.01, distance) * float2(1.0, 1.0); 
}

Out = result;