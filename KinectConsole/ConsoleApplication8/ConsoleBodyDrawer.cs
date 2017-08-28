using ConsoleExtensions;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplication8
{
  class ConsoleBodyDrawer : IBodyDrawer
  {
    public ConsoleBodyDrawer()
    {
      this.Color = ConsoleColor.Green;
    }
    public ConsoleColor Color { get; set; }

#if ZERO
    public void DrawFrame(Body body, CoordinateMapper mapper,
      Rect depthFrameSize,
      bool isMain)
#else
    public void DrawFrame(Body body, CoordinateMapper mapper,
      Rect depthFrameSize)
#endif
    {
      this.Resize();

      foreach (var jointType in interestedJointTypes)
      {
        var joint = body.Joints[jointType];

        if (joint.TrackingState != TrackingState.NotTracked)
        {
          var cameraPosition = joint.Position;

          DepthSpacePoint depthPosition = mapper.MapCameraPointToDepthSpace(cameraPosition);

          if (!float.IsNegativeInfinity(depthPosition.X))
          {
            var consolePosition = MapDepthPointToConsoleSpace(depthPosition, depthFrameSize);

            ConsoleEx.DrawAt(
              consolePosition.Item1,
              consolePosition.Item2,
              jointType.ToString().Substring(0,1),
              joint.TrackingState == TrackingState.Inferred ? ConsoleColor.Gray : this.Color);
          }
        }
      }
#if ZERO
      if (isMain)
      {
        DumpMainBodyInfo(body);
      }
#endif
    }
    void Resize()
    {
      if (!resized)
      {
        Console.SetWindowSize(Constants.ConsoleWidth, Constants.ConsoleHeight);
        resized = true;
      }
    }
    static Tuple<int, int> MapDepthPointToConsoleSpace(DepthSpacePoint depthPoint,
      Rect depthFrameSize)
    {
      var left = (int)((depthPoint.X / (double)depthFrameSize.Width) * (Constants.ConsoleWidth - 1));
      var top = (int)((depthPoint.Y / (double)depthFrameSize.Height) * (Constants.ConsoleHeight - 1));

      left = Math.Min(left, Constants.ConsoleWidth - 1);
      top = Math.Min(top, Constants.ConsoleHeight - 1);
      left = Math.Max(0, left);
      top = Math.Max(0, top);
      return (Tuple.Create(left, top));
    }
    static JointType[] interestedJointTypes = 
    {
      JointType.Head,
      JointType.Neck,
      JointType.ShoulderLeft,
      JointType.ShoulderRight,
      JointType.HandLeft,
      JointType.HandRight,
      JointType.ElbowLeft,
      JointType.ElbowRight,
      JointType.HipLeft,
      JointType.HipRight,
      JointType.KneeLeft,
      JointType.KneeRight,
      JointType.AnkleLeft,
      JointType.AnkleRight,
      JointType.FootLeft,
      JointType.FootRight
    };
    static bool resized = false;

#if ZERO
    void DumpMainBodyInfo(Body body)
    {
      List<Tuple<String, DetectionResult>> bodyInfo = new List<Tuple<string, DetectionResult>>();
      bodyInfo.Add(Tuple.Create("engaged", body.Engaged));

      foreach (var activity in body.Activities)
      {
        bodyInfo.Add(Tuple.Create(activity.Key.ToString(), activity.Value));
      }
      foreach (var appearance in body.Appearance)
      {
        bodyInfo.Add(Tuple.Create(appearance.Key.ToString(),
          appearance.Value));
      }
      foreach (var expression in body.Expressions)
      {
        bodyInfo.Add(Tuple.Create(expression.Key.ToString(),
          expression.Value));
      }
      int position = 0;

      // dump all those out if they are positive
      for (int i = 0; i < bodyInfo.Count; i++)
      {
        if (
          (bodyInfo[i].Item2 != DetectionResult.No) &&
          (bodyInfo[i].Item2 != DetectionResult.Unknown))
        {
          ConsoleEx.DrawAt(
            i,
            0,
            bodyInfo[i].Item1,
            bodyInfo[i].Item2 == DetectionResult.Yes ? ConsoleColor.Yellow : ConsoleColor.Gray);

          position++;
        }
      }
    }
#endif
  }
}
