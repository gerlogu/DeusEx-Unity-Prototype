#ifndef SDF_2D
#define SDF_2D

half circle(half2 _samplePosition, half _radius){
    return length(_samplePosition) - _radius;
}

half rectanlge(half2 _samplePosition, half _width, half _height){
    half2 d = abs(_samplePosition) - half2(_width, _height) / 2.0;
    half sdf = min(max(d.x, d.y), 0.0) + length(max(d, 0.0));
    return sdf;
}

//Credit: https://www.shadertoy.com/view/XdXcRB | MIT License
float ndot(float2 a, float2 b ) { return a.x*b.x - a.y*b.y; }
float sdRhombus(float2 p, float2 b) {
    float2 q = abs(p);
    float h = clamp((-2.0*ndot(q,b)+ndot(b,b))/dot(b,b),-1.0,1.0);
    float d = length( q - 0.5*b*float2(1.0-h,1.0+h) );
    return d * sign( q.x*b.y + q.y*b.x - b.x*b.y );
}
//EndCredit

//Credit: https://www.shadertoy.com/view/MldcD7 | MIT License
float sdTriangleIsosceles(float2 p, float2 q )
{
    p.x = abs(p.x);
    float2 a = p - q*clamp( dot(p,q)/dot(q,q), 0.0, 1.0 );
    float2 b = p - q*float2( clamp( p.x/q.x, 0.0, 1.0 ), 1.0 );
    float s = -sign( q.y );
    float2 d = min( float2( dot(a,a), s*(p.x*q.y-p.y*q.x) ), float2( dot(b,b), s*(p.y-q.y)  ));
    return -sqrt(d.x)*sign(d.y);
}
//EndCredit

//Credit: https://www.shadertoy.com/view/3tSGDy | MIT License
float sdNStarPolygon(in float2 p, in float r, in float n, in float m) // m=[2,n]
{
    float an = 3.141593/float(n);
    float en = 3.141593/m;
    float2  acs = float2(cos(an),sin(an));
    float2  ecs = float2(cos(en),sin(en));
    float bn = abs(atan2(p.x, p.y)) % (2.0*an) - an;
    p = length(p)*float2(cos(bn),abs(sin(bn)));
    p -= r*acs;
    p += ecs*clamp( -dot(p,ecs), 0.0, r*acs.y/ecs.y);
    return length(p)*sign(p.x);
}
//EndCredit

half sampleSdf(half _sdf, half _offset){
    half sdf = saturate(-_sdf * _offset);
    return sdf;
}

half sampleSdfStrip(half _sdf, half _stripWidth, half _offset){
   
    half l = (_stripWidth+1.0/_offset)/2.0;
	return saturate((l-distance(-_sdf,l))*_offset);
}

half sdfUnion(half _a, half _b){
    return max(_a, _b);
}

half sdfIntersection(half _a, half _b){
    return min(_a, _b);
}

half sdfDifference(half _a, half _b)
{
    return max(_a, -_b);
}

half map(half val, half low1, half high1, half low2, half high2){
    return low2 + (val - low1) * (high2 - low2) / (high1 - low1);
}

#endif