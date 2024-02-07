using UnityEngine;

namespace Services
{
    public interface IScreenSizeProvider
    {
        float ScreenWorldHeight { get; }
        float ScreenWorldWidth { get; }
    }

    class ScreenSizeProvider : IScreenSizeProvider
    {
        public float ScreenWorldHeight { get; }
        public float ScreenWorldWidth { get; }
        
        public ScreenSizeProvider(Camera camera)
        {
            ScreenWorldHeight = GetScreenWorldHeight(camera);
            ScreenWorldWidth = GetScreenWorldWidth(camera);
        }

        private float GetScreenWorldHeight(Camera camera)
        {
            float cameraHeight = camera.orthographicSize * 2;
            return cameraHeight;
        }

        private float GetScreenWorldWidth(Camera camera)
        {
            float cameraHeight = ScreenWorldHeight;
            float cameraWidth = cameraHeight * camera.aspect;
            return cameraWidth;
        }
    }
}