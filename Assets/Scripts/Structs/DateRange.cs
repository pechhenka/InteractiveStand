using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stand
{
    public struct DateRange
    {
        public DateTime Left;
        public DateTime Right;
        public bool Twins;

        public DateRange(bool Twins, DateTime Left, DateTime Right = default)
        {
            this.Twins = Twins;
            this.Left = Left;
            this.Right = Right;
        }
    }
}