﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SZJS.ElectronTicket.Task
{
    /// <summary>
    /// 选项值类
    /// </summary>
    public class OptionValue : Object
    {
        public object Value;

        public OptionValue(object value)
        {
            Value = value;
        }

        public string ToString(string DefaultValue)
        {
            string Result = DefaultValue;

            try
            {
                Result = Convert.ToString(Value);
            }
            catch { }

            return Result;
        }

        public bool ToBoolean(bool DefaultValue)
        {
            bool Result = DefaultValue;

            try
            {
                Result = Convert.ToBoolean(Value);
            }
            catch { }

            return Result;
        }

        public short ToShort(short DefaultValue)
        {
            short Result = DefaultValue;

            try
            {
                Result = Convert.ToInt16(Value);
            }
            catch { }

            return Result;
        }

        public int ToInt(int DefaultValue)
        {
            int Result = DefaultValue;

            try
            {
                Result = Convert.ToInt32(Value);
            }
            catch { }

            return Result;
        }

        public long ToLong(long DefaultValue)
        {
            long Result = DefaultValue;

            try
            {
                Result = Convert.ToInt64(Value);
            }
            catch { }

            return Result;
        }

        public Double ToDouble(Double DefaultValue)
        {
            Double Result = DefaultValue;

            try
            {
                Result = Convert.ToDouble(Value);
            }
            catch { }

            return Result;
        }

        public DateTime ToDateTime(DateTime DefaultValue)
        {
            DateTime Result = DefaultValue;

            try
            {
                Result = Convert.ToDateTime(Value);
            }
            catch { }

            return Result;
        }
    }
}
