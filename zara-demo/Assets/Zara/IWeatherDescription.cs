namespace ZaraEngine {

    public interface IWeatherDescription {

        float RainIntensity { get; } // 0..1
        float Temperature { get; }   // degrees C
        float WindSpeed { get; }     // 0..1

    }

}