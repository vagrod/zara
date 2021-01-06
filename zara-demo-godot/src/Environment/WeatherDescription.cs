using ZaraEngine;

public class WeatherDescription : IWeatherDescription 
{

    private float _rainIntensity;
    private float _temperature;
    private float _windSpeed;

    public float RainIntensity => _rainIntensity;
    public float Temperature => _temperature;
    public float WindSpeed => _windSpeed;

    public void SetRainIntensity(float value){
        _rainIntensity = value;
    }

    public void SetTemperature(float value){
        _temperature = value;
    }

    public void SetWindSpeed(float value){
        _windSpeed = value;
    }

}
