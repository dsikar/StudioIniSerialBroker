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

     // Code tracking
    public Stack<string> stack = new Stack<string>();
    public List<string> strList = new List<string>()
    {
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0",
            "0"
    };

    public int iStackCount = 10;
    public int iListCount = 40;

    // CODE LIMITS
    private int iCode1LowY = 20;
    private int iCode1HighY = 30; 

    enum MyEnum
    {
        CODE0,
        CODE1,
        CODE2,
        CODE3,
        CODE4,
        CODE5,
        CODE6
    }

    private void PrintStack(int iCode, int iLHX, int iLHY, int iRHX, int iRHY)
    {
        // Pop and insert
        int i;
        for(i = 0; i < iListCount - 1; i++)
        {
            strList[i] = strList[i+1];
        }
        strList[i] = iCode.ToString() + iLHX.ToString() + iLHY.ToString() + iRHX.ToString() + iRHY.ToString();

        for(int r = 0; r < iListCount; r++)
        {
            ConsoleEx.DrawAt(1, 4 + r, strList[r], ConsoleColor.Black);
        }
    }

    public ConsoleBodyDrawer()
    { 
      this.Color = ConsoleColor.Green;
    }

    public ConsoleColor Color { get; set; }

    public int getCode(int[] iXY)
    {
        if(Constants.bColour == false)
        {
          Console.BackgroundColor = ConsoleColor.White;
        }
        int iRetVal = (int)MyEnum.CODE0;
        // CODE1
//        if (((iXY[idxLY] >= 20 && iXY[idxLY] <= 30) && (iXY[idxRY] >= 20 && iXY[idxRY] <= 30))
//                && ((iXY[idxLX] >= 40 && iXY[idxLX] <= 90) && (iXY[idxRX] >= 40 && iXY[idxRX] <= 90)))
        if (((iXY[idxRY] - iXY[idxLY]  >= -5) && (iXY[idxRY] - iXY[idxLY]  < 20) )
                && ((iXY[idxRX] - iXY[idxLX] >= 0) && (iXY[idxRX] - iXY[idxLX] < 20)))
        {
            if(Constants.bColour == true)
            {
              Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
            return (int)MyEnum.CODE1;
        }
        // CODE2
        if (((iXY[idxRY] - iXY[idxLY]  >= -5) && (iXY[idxRY] - iXY[idxLY]  < 20) )
                && ((iXY[idxRX] - iXY[idxLX] > 20) && (iXY[idxRX] - iXY[idxLX] < 100)))
        {
            if(Constants.bColour == true)
            {
              Console.BackgroundColor = ConsoleColor.DarkCyan;
            }
            return (int)MyEnum.CODE2;
        }
        // CODE4
        if (((iXY[idxRY] - iXY[idxLY]  > -5) && (iXY[idxRY] - iXY[idxLY]  < 40) )
                && ((iXY[idxRX] - iXY[idxLX] > 0) && (iXY[idxRX] - iXY[idxLX] < 100)))
        {
            if(Constants.bColour == true)
            {
              Console.BackgroundColor = ConsoleColor.Cyan;
            }
            return (int)MyEnum.CODE4;
        }
        // CODE3
        if (((iXY[idxRY] - iXY[idxLY]  < -5) && (iXY[idxRY] - iXY[idxLY]  > -40) )
                && ((iXY[idxRX] - iXY[idxLX] > 0) && (iXY[idxRX] - iXY[idxLX] < 100)))
        {
            if(Constants.bColour == true)
            {
              Console.BackgroundColor = ConsoleColor.Blue;
            }
            return (int)MyEnum.CODE3;
        }
        // CODE5 - Right hand over left hand
        if (((iXY[idxRY] - iXY[idxLY]  < -5) && (iXY[idxRY] - iXY[idxLY]  > -40) )
                && ((iXY[idxRX] - iXY[idxLX] < -5) && (iXY[idxRX] - iXY[idxLX] > - 100)))
        {
            if(Constants.bColour == true)
            {
              Console.BackgroundColor = ConsoleColor.Green;
            }
            return (int)MyEnum.CODE5;
        }
        // CODE6 - left hand over right hand 
        if (((iXY[idxRY] - iXY[idxLY]  > -5) && (iXY[idxRY] - iXY[idxLY]  < 40) )
                && ((iXY[idxRX] - iXY[idxLX] > -100) && (iXY[idxRX] - iXY[idxLX] < -5)))
        {
            if(Constants.bColour == true)
            {
              Console.BackgroundColor = ConsoleColor.DarkGreen;
            }
            return (int)MyEnum.CODE6;
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
      if (Constants.bPaused == true)
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
              // draw lines - MAKES DISPLAY FLICKER TOO MUCH
              // DrawLines();
              // swapped  jointType.ToString().Substring(0,1) for strPrintout
              ConsoleEx.DrawAt(
                consolePosition.Item1,
                consolePosition.Item2,
                strPrintout,
                joint.TrackingState == TrackingState.Inferred ? ConsoleColor.Black : ConsoleColor.Black);

              // draw code
              ConsoleEx.DrawAt(1, 1, "Code Left = " + iXY[idxLX].ToString() + ", " + iXY[idxLY].ToString(), ConsoleColor.Black);
              ConsoleEx.DrawAt(1, 2, "Code Right = " + iXY[idxRX].ToString() + ", " + iXY[idxRY].ToString(), ConsoleColor.Black);
              ConsoleEx.DrawAt(1, 3, "CODE = " + iCode.ToString(), ConsoleColor.Black);

              PrintStack(iCode, iXY[idxLX], iXY[idxLY], iXY[idxRX], iXY[idxRY]);
              // draw border lines
              // Console.BackgroundColor = ConsoleColor.DarkRed;
            }
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
