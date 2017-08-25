using Microsoft.Kinect;
using System;

namespace ConsoleApplication8
{
  interface IBodyDrawer
  {
    ConsoleColor Color { get; set; }

#if ZERO
    void DrawFrame(Body body, CoordinateMapper mapper, Rect depthFrameSize, bool isMain);
#else
    void DrawFrame(Body body, CoordinateMapper mapper, Rect depthFrameSize);
#endif
  }
}
