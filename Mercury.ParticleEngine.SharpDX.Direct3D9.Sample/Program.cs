﻿namespace Mercury.ParticleEngine
{
    using System;
    using System.Diagnostics;
    using SharpDX;
    using SharpDX.Direct3D9;
    using SharpDX.Windows;
    using Mercury.ParticleEngine.Profiles;
    using Mercury.ParticleEngine.Renderers;

    static class Program
    {
        [STAThread]
        static void Main()
        {
            var form = new RenderForm("Mercury Particle Engine - SharpDX.Direct3D9 Sample");

            var direct3d = new Direct3D();
            var device = new Device(direct3d, 0, DeviceType.Hardware, form.Handle, CreateFlags.HardwareVertexProcessing, new PresentParameters(form.ClientSize.Width, form.ClientSize.Height));

            var view = new Matrix(
                1.0f, 0.0f, 0.0f, 0.0f,
                0.0f, -1.0f, 0.0f, 0.0f,
                0.0f, 0.0f, -1.0f, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f);
            var proj = Matrix.OrthoOffCenterLH(form.ClientSize.Width * -0.5f, form.ClientSize.Width * 0.5f, form.ClientSize.Height * 0.5f, form.ClientSize.Height * -0.5f, 0f, 100f);

            var emitter = new Emitter(50000, 3.0f, Profile.Point())
            {
                Parameters = new ReleaseParameters
                {
                    Colour   = new ColourRange(new RangeF(0f, 0.3f), new RangeF(0.3f, 0.7f), new RangeF(0.8f, 1f)),
                    Opacity  = new RangeF(1f, 1f),
                    Quantity = new Range(250, 250),
                    Speed    = new RangeF(0f, 1f),
                    Scale    = new RangeF(32f, 32f)
                }
            };
            var renderer = new PointSpriteRenderer(device, emitter);

            var texture = Texture.FromFile(device, "Particle.dds");

            var stopwatch = Stopwatch.StartNew();
            var totalTime = 0f;

            RenderLoop.Run(form, () =>
                {
                    stopwatch.Reset();
                    stopwatch.Start();

                    emitter.Trigger((float)Math.Sin(totalTime) * 350f, 0f);
                    emitter.Update(0.01666666f);

// ReSharper disable AccessToDisposedClosure
                    device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1f, 0);
                    device.BeginScene();

                    renderer.Render(Matrix.Identity * view * proj, texture);

                    device.EndScene();
                    device.Present();
// ReSharper restore AccessToDisposedClosure

                    form.Text = String.Format("{0:G} :: {1}", emitter.ActiveParticles, stopwatch.Elapsed.TotalSeconds.ToString());

                    totalTime += (float)stopwatch.Elapsed.TotalSeconds;

                });

            device.Dispose();
            direct3d.Dispose();
        }
    }
}