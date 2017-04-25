﻿using System;
using Android.Support.V4.App;
using System.Runtime.Remoting.Messaging;

namespace InnovifySample.Droid
{
  public class Adapter : FragmentPagerAdapter
  {

    public Adapter(FragmentManager fm) : base(fm) { }

    public override int Count => 3;

    public override Fragment GetItem(int position)
    {
      switch (position)
      {
        case 0 : return new Welcome();
        case 1 : return new Contact();
        case 2 : return new Bye();
          
        default: return null;
      }
    }
  }
}
