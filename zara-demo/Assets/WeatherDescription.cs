using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZaraEngine;
using ZaraEngine.HealthEngine;

public class WeatherDescription : IWeatherDescription 
{

    public float RainIntensity => 0f;
    public float Temperature => 25f;
    public float WindSpeed => 0.1f;

}