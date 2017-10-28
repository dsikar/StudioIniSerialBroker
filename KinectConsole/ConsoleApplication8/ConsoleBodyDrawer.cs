using ConsoleExtensions;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplication8
{
  class ConsoleBodyDrawer : IBodyDrawer
  {
    private int[] iXY= { 0, 0, 0, 0 };
    private int iTrack = 0;
    private int iMax = 2;
    private int idxLX = 0;
    private int idxLY = 1;
    private int idxRX = 2;
    private int idxRY = 3;
   
    // CODE LIMITS
    private int iCode1LowY = 20;
    private int iCode1HighY = 30; 

    enum MyEnum
    {
        CODE0,
        CODE1,
        CODE2,
        CODE3,
        CODE4
    }

    public ConsoleBodyDrawer()
    {
      this.Color = ConsoleColor.Green;
    }
    public ConsoleColor Color { get; set; }

    public int getCode(int[] iXY)
    {
        int iRetVal = (int)MyEnum.CODE0;
        // CODE1
        if (((iXY[idxLY] >= 20 && iXY[idxLY] <= 30) && (iXY[idxRY] >= 20 && iXY[idxRY] <= 30))
                && ((iXY[idxLX] >= 50 && iXY[idxLX] <= 70) && (iXY[idxRX] >= 70 && iXY[idxRX] <= 90)))
        {
            return (int)MyEnum.CODE1;
        }
        // CODE2
        if (((iXY[idxLY] >= 20 && iXY[idxLY] <= 30) && (iXY[idxRY] >= 20 && iXY[idxRY] <= 30))
                && ((iXY[idxLX] >= 10 && iXY[idxLX] <= 50) && (iXY[idxRX] > 90 && iXY[idxRX] < 120)))
        {
            return (int)MyEnum.CODE2;
        }
        // CODE3
        if (((iXY[idxLY] >= 0 && iXY[idxLY] <= 24) && (iXY[idxRY] > 24 && iXY[idxRY] <= 40))
                && ((iXY[idxLX] >= 5 && iXY[idxLX] <= 70) && (iXY[idxRX] > 70 && iXY[idxRX] < 120)))
        {
            return (int)MyEnum.CODE4;
        }
        // CODE4
        if (((iXY[idxLY] > 24 && iXY[idxLY] <= 40) && (iXY[idxRY] >= 0 && iXY[idxRY] <= 24))
                && ((iXY[idxLX] >= 5 && iXY[idxLX] <= 70) && (iXY[idxRX] > 70 && iXY[idxRX] < 120)))
        {
            return (int)MyEnum.CODE3;
        }
            return iRetVal;
    }

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
            // Ajust index
            if(iTrack > iMax)
            {
              iTrack = 0;
            }
            // Populate holders
            iXY[iTrack] = consolePosition.Item1;
            iXY[iTrack+1] = consolePosition.Item2;
            String strPrintout = (iTrack == 0 ? "LeftHand" : "RightHand");
            int iCode = getCode(iXY);
            iTrack += 2;
            // swapped  jointType.ToString().Substring(0,1) for strPrintout
            ConsoleEx.DrawAt(
              consolePosition.Item1,
              consolePosition.Item2,
              strPrintout,
              joint.TrackingState == TrackingState.Inferred ? ConsoleColor.Gray : this.Color);
            // draw code
            ConsoleEx.DrawAt(1, 1, "Code Left = " + iXY[idxLX].ToString() + ", " + iXY[idxLY].ToString(), ConsoleColor.Green);
            ConsoleEx.DrawAt(1, 2, "Code Right = " + iXY[idxRX].ToString() + ", " + iXY[idxRY].ToString(), ConsoleColor.Green);
            ConsoleEx.DrawAt(1, 3, "CODE = " + iCode.ToString(), ConsoleColor.Green);
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
        Console.SetWindowSize(150, 50);
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
      //JointType.Head,
      //JointType.Neck,
      //JointType.ShoulderLeft,
      //JointType.ShoulderRight,
      JointType.HandLeft,
      JointType.HandRight
      //JointType.ElbowLeft,
      //JointType.ElbowRight,
      //JointType.HipLeft,
      //JointType.HipRight,
      //JointType.KneeLeft,
      //JointType.KneeRight,
      //JointType.AnkleLeft,
      //JointType.AnkleRight,
      //JointType.FootLeft,
      //JointType.FootRight
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
