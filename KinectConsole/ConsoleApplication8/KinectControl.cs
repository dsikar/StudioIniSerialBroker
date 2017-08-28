using Microsoft.Kinect;
using System;

namespace ConsoleApplication8
{
  class Rect
  {
    public int Width;
    public int Height;
  }
  class KinectControl
  {
    public KinectControl(Func<IBodyDrawer> bodyDrawerFactory)
    {
      this.bodyDrawers = new IBodyDrawer[colours.Length];

      for (int i = 0; i < colours.Length; i++)
      {
        this.bodyDrawers[i] = bodyDrawerFactory();
        this.bodyDrawers[i].Color = colours[i];
      }
    }
    public Rect DepthFrameSize
    {
      get;
      private set;
    }
    public void GetSensor()
    {
      this.sensor = KinectSensor.GetDefault();
      this.sensor.Open();
      this.DepthFrameSize = new Rect()
      {
        Width = this.sensor.DepthFrameSource.FrameDescription.Width,
        Height = this.sensor.DepthFrameSource.FrameDescription.Height
      };      
    }
    public void OpenReader()
    {
      this.reader = this.sensor.BodyFrameSource.OpenReader();
      this.reader.FrameArrived += OnFrameArrived;
    }
    public void CloseReader()
    {
      this.reader.FrameArrived -= OnFrameArrived;
      this.reader.Dispose();
      this.reader = null;
    }
    void OnFrameArrived(object sender, BodyFrameArrivedEventArgs e)
    {
      using (var frame = e.FrameReference.AcquireFrame())
      {
        if ((frame != null) && (frame.BodyCount > 0))
        {
          if ((this.bodies == null) || (this.bodies.Length != frame.BodyCount))
          {
            this.bodies = new Body[frame.BodyCount];
          }
          frame.GetAndRefreshBodyData(this.bodies);

          Console.Clear();

#if ZERO
          bool first = true;
#endif

          for (int i = 0; i < colours.Length; i++)
          {
            if (this.bodies[i].IsTracked)
            {

#if ZERO
              this.bodyDrawers[i].DrawFrame(
                this.bodies[i],
                this.sensor.CoordinateMapper,
                this.DepthFrameSize,
                first);

              first = false;
#else
              this.bodyDrawers[i].DrawFrame(
                 this.bodies[i],
                 this.sensor.CoordinateMapper,
                 this.DepthFrameSize);
#endif
            }
          }
        }
      }
    }
    public void ReleaseSensor()
    {
      this.sensor.Close();
      this.sensor = null;
    }
    Body[] bodies;
    KinectSensor sensor;
    BodyFrameReader reader;
    IBodyDrawer[] bodyDrawers;

    static ConsoleColor[] colours = 
    {
        ConsoleColor.Red,
        ConsoleColor.White,
        ConsoleColor.Green,
        ConsoleColor.Yellow,
        ConsoleColor.Cyan,
        ConsoleColor.Gray
    };
  }
}