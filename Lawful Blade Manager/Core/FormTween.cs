using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawfulBladeManager.Core
{
    public static class FormTween
    {
        // Panel Extensions...
        public static void Slide(this Panel panel, Point target, float time)
        {
            // We use tasks to do our tweening...
            Task task = new Task(() =>
            {
                // Cache the old location...
                Point oldLocation = panel.Location;
                Point newLocation = panel.Location;

                Stopwatch stopwatch = Stopwatch.StartNew();
                float lerpTime = 0f, cF = 0f;
                do
                {
                    // Consine Lerp from A to B...
                    cF = (1 - MathF.Cos((lerpTime / time) * MathF.PI)) / 2f;
                    newLocation.X = (int)(oldLocation.X * (1f - cF) + target.X * cF);
                    newLocation.Y = (int)(oldLocation.Y * (1f - cF) + target.Y * cF);

                    // Set new location
                    panel.Invoke(() => panel.Location = newLocation);

                    // Set lerp time
                    lerpTime = (stopwatch.ElapsedTicks / (float)TimeSpan.TicksPerSecond);

                } while (lerpTime < time);

                stopwatch.Stop();
            });
            task.ContinueWith((Task obj) =>
            {
                panel.Location = target;
            });

            task.Start();
        }
    }
}